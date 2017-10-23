using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace SocketClipboard
{
    public partial class Main : Form
    {

        private NetListener server;
        private List<NetClient> clients = new List<NetClient>();

        private Thread thread_emitter, thread_listener;

        //static byte[] MultipacketBuffer = new byte[Utility.MultiPacketCap];
        static BinaryFormatter BinFormatter = new BinaryFormatter();

        void ThreadListener()
        {
            while (true)
            {
                if (config.Offline | config.Mute) { Thread.Sleep(1000); }

                if (server.Active && server.Pending())
                {
                    Log(NotificationType.Receiving);

                    var stream = server.AcceptTcpClient().GetStream();

//                    try
                    {
                        ClipBuffer f = BinFormatter.Deserialize(stream) as ClipBuffer;

                        if (f == null) throw new InvalidOperationException("Can't listen to unrecognizable packet (suggest to switch port?)");

                        // Clip Freeze prevents the next copy buffer event makes problem.
                        clip_freeze = true;

                        if (f.Type == DataType.Files)
                        {
                            var files = f as FileBuffer;

                            Utility.SetupTemporaryFiles(files);

                            progresser.Init(files);

                            ListenToFiles(stream, files);

                            progresser.Done();
                        }

                        Invoke(new Action(f.ToClipboard));

                        Thread.Sleep(100);

                        Log(NotificationType.Received, f);
                    }

                    //catch (Exception ex) { Log("Listening failed: " + ex.Message); continue; }
                    
                    stream.Close();
                }

                Thread.Sleep(50);

                clip_freeze = false;
            }
        }

        private void ListenToFiles(NetworkStream stream, FileBuffer files)
        {
            ListenBytes.Clear();
            ListenBytesDone = false;
            ThreadPool.QueueUserWorkItem((x) => ListenFilesWrite(files));

            while (!ListenBytesDone)
            {
                ListenBytes.Enqueue(stream);
            }
        }

        StreamQueue ListenBytes = new StreamQueue();
        bool ListenBytesDone = false;

        static int SafeSubstract(long a, long b)
        {
            return (a - b > int.MaxValue ? int.MaxValue : (int)(a - b));
        }

        void ListenFilesWrite(FileBuffer files)
        {
            int iterF = 0; long iterB = 0;
            // try
            {
                foreach (var file in files.files)
                {
                    progresser.Update(iterF++, file.name);
                    using (var io = File.Create(file.destination))
                    {
                        //  using (var iob = new BufferedStream(io, 1024 * 128))
                        {
                            long size = 0; long count;
                            while (size < file.size)
                            {
                                // while (ListenBytes.Count == 0 && !ListenBytesDone) { }

                                if (ListenBytes.Count > 0)
                                {
                                    size += count = ListenBytes.Dequeue(io, SafeSubstract(file.size, size));
                                    progresser.Update(iterB += count);
                                }
                            }
                        }
                    }
                    File.SetLastWriteTime(file.destination, file.modified);
                }
            }
            //catch (Exception) { }
            ListenBytesDone = true;
        }

        void ThreadEmitter()
        {
            while (true)
            {

                Thread.Sleep(50);

                if (!clip_dirty)
                    continue;

                clip_dirty = false;

                Log(NotificationType.Sending);

                for (int i = 0; i < clients.Count; i++)
                {
                    var tcp = clients[i];
                    var ok = true;

                    if (config.Solo && soloIndex != i) { tcp.Log = "..."; continue; }

                    try
                    {
                        var task = (tcp.ConnectAsync(tcp.Name, config.Port));
                        tcp.Log = (ok = task.Wait(1000)) ? "Sent" : "Failed";

                        if (ok)
                        {
                            var stream = tcp.GetStream();
                            BinFormatter.Serialize(stream, clip_data);

                            if (clip_data.Type == DataType.Files)
                            {
                                // The story doesn't end there..
                                // Send the file buffer

                                EmitFiles(stream);
                            }

                            stream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        tcp.Log = "Failed: " + ex.DeepMessage();
                    }

                    // Close and set the new one
                    tcp.Close();
                    clients[i] = new NetClient(tcp.Name) { Log = tcp.Log };
                }

                Log(NotificationType.Sent, clip_data);
            }
        }

        private void EmitFiles(NetworkStream stream)
        {
            EmitBytesDone = false;
            EmitBytes.Clear();
            ThreadPool.QueueUserWorkItem((x) => EmitFilesRead(clip_data as FileBuffer));

            while (!EmitBytesDone)
            {
                while (EmitBytes.Count > 0)
                {
                    EmitBytes.Dequeue(stream);
                }
            }
        }

        StreamQueue EmitBytes = new StreamQueue();
        bool EmitBytesDone = false;

        void EmitFilesRead(FileBuffer files)
        {
            try
            {
                foreach (var file in files.files)
                    using (var io = File.OpenRead(file.source))
                    {
                        var size = 0;
                        while ((size += EmitBytes.Enqueue(io, SafeSubstract(file.size, size))) < file.size) { }
                    }

            }
            catch (Exception) { }
            EmitBytesDone = true;
        }

        void StartThreader()
        {
            (thread_emitter = new Thread(ThreadEmitter) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
            (thread_listener = new Thread(ThreadListener) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
        }

        void StartServer()
        {
            if (server != null) { server.Stop(); server.Server.Dispose(); }

            var ip = Utility.GetLocalIPAddress();

            (server = ip != null ? new NetListener(ip, config.Port) : null)?.Start();
        }

    }
}
