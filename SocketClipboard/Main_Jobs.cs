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
        private Dictionary<string, string> logs = new Dictionary<string, string>();

        private Thread thread_emitter;
        private Thread thread_listener;

        private ManualResetEvent manual_emitter;
        private ManualResetEvent manual_listener;

        //  static byte[] UnipacketBuffer = new byte[Utility.SinglePacketCap];
        static byte[] MultipacketBuffer = new byte[Utility.MultiPacketCap];
        static BinaryFormatter BinFormatter = new BinaryFormatter();

        void ThreadListener()
        {
            while (true)
            {
                if (server.Active && server.Pending())
                {
                    Log(NotificationType.Receiving);

                    var tcp = server.AcceptTcpClient();
                    var stream = tcp.GetStream();
                    {
                        ClipBuffer f = null;
                        try
                        {
                            f = BinFormatter.Deserialize(stream) as ClipBuffer;

                            if (f != null)
                            {
                                // Clip Freeze prevents the next copy buffer event makes problem.
                                clip_freeze = true;

                                if (f.Type != DataType.Files)
                                {
                                    Invoke(new Action(f.ToClipboard));
                                    Thread.Sleep(100);
                                }
                                else
                                {
                                    var files = f as FileBuffer;
                                    int iterF = 0; long iterB = 0;
                                    Utility.SetupTemporaryFiles(files);

                                    progresser.Init(files);

                                    // Here comes the second wave...

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

                                    progresser.Done();

                                    Invoke(new Action(f.ToClipboard));
                                    Thread.Sleep(100);
                                }

                            }

                            Log(NotificationType.Received, f);
                        }

                        catch (Exception ex) { Log("Listening failed: " + ex.Message); continue; }

                        stream.Close();
                    }
                }

                Thread.Sleep(50);
                manual_listener.WaitOne();
                clip_freeze = false;
            }
        }

        void ThreadEmitter()
        {
            while (true)
            {

                Thread.Sleep(50);
                manual_emitter.WaitOne();

                if (!clip_dirty)
                    continue;

                clip_dirty = false;
                Log(NotificationType.Sending);

                for (int i = 0; i < clients.Count; i++)
                {
                    var tcp = clients[i];
                    var ok = true;
                    try
                    {
                        var task = (tcp.ConnectAsync(tcp.Name, port));
                        ok = task.Wait(1000);
                        logs[tcp.Name] = ok ? "Sent" : "Failed";

                        if (ok)
                        {
                            var stream = tcp.GetStream();
                            BinFormatter.Serialize(stream, clip_data);

                            if (clip_data.Type == DataType.Files)
                            {
                                // The story doesn't end there..
                                // Send the file buffer

                                var files = clip_data as FileBuffer;
                                foreach (var file in files.files)
                                {
                                    var path = file.name;


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
                            stream.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        logs[tcp.Name] = "Failed: " + (ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                    }

                    // Close and set the new one
                    tcp.Close();
                    clients[i] = new NetClient(tcp.Name);
                }

                Log(NotificationType.Sent, clip_data);
            }
        }


        void StartThreader()
        {
            manual_emitter = new ManualResetEvent(true);
            manual_listener = new ManualResetEvent(true);

            (thread_emitter = new Thread(ThreadEmitter) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
            (thread_listener = new Thread(ThreadListener) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
        }

        void SetThreader(bool active)
        {
            if (active)
            {
                manual_listener.Set();
                manual_emitter.Set();
            }
            else
            {
                manual_listener.Reset();
                manual_emitter.Reset();
            }
        }

        void StartServer()
        {
            if (server != null)
                server.Stop();

            server = new NetListener(Utility.GetLocalIPAddress(), port);
            server.Start();
        }

    }
}
