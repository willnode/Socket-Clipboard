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

        static byte[] MultipacketBuffer = new byte[Utility.MultiPacketCap];
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

                    try
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

                    catch (Exception ex) { Log("Listening failed: " + ex.Message); continue; }

                    stream.Close();
                }

                Thread.Sleep(50);

                clip_freeze = false;
            }
        }

        private void ListenToFiles(NetworkStream stream, FileBuffer files)
        {
            int iterF = 0; long iterB = 0;

            foreach (var file in files.files)
            {
                progresser.Update(iterF++, file.name);

                if (file.multiStaged)
                {
                    using (var io = File.Create(file.destination))
                    {
                        long size = 0;
                        int count = 0;
                        while (size < file.size && (count = stream.Read(MultipacketBuffer, 0, MultipacketBuffer.Length)) > 0)
                        {
                            io.Write(MultipacketBuffer, 0, count);
                            size += count;
                            progresser.Update(iterB += count);
                        }
                    }
                }
                else
                {
                    File.WriteAllBytes(file.destination, BinFormatter.Deserialize(stream) as byte[]);
                    progresser.Update(iterB += file.size);
                }

                File.SetLastWriteTime(file.destination, file.modified);
            }
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
            var files = clip_data as FileBuffer;
            foreach (var file in files.files)
            {
                if (file.multiStaged)
                {
                    using (var io = File.OpenRead(file.source))
                    {
                        long size = 0;
                        int count = 0;
                        while (size < file.size && (count = io.Read(MultipacketBuffer, 0, MultipacketBuffer.Length)) > 0)
                        {
                            stream.Write(MultipacketBuffer, 0, count);
                            size += count;
                        }
                    }
                }
                else
                {
                    BinFormatter.Serialize(stream, File.ReadAllBytes(file.source));
                }
            }
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
