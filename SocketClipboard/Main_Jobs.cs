using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Diagnostics;

namespace SocketClipboard
{
    public class MultipacketSwap
    {
        public byte[] data1 = new byte[Utility.MultiPacketCap];
        public byte[] data2 = new byte[Utility.MultiPacketCap];

        public int disk_iter;
        public int netw_iter;
        public bool listening;
        public bool finished;

        public byte[] GetForDisk()
        {
            if (listening) while (netw_iter < disk_iter + 1 & !finished) { Thread.Sleep(0); } // Block until network finishes
            else while (netw_iter < disk_iter & !finished) { Thread.Sleep(0); }
            return disk_iter % 2 == 0 ? data1 : data2;
        }

        public byte[] GetForNetw()
        {
            if (listening) while (disk_iter < netw_iter & !finished) { Thread.Sleep(0); } // Block until disk finishes
            else while (disk_iter < netw_iter + 1 & !finished) { Thread.Sleep(0); }
            return netw_iter % 2 == 0 ? data1 : data2;
        }

        public void DiskDone()
        {
            disk_iter++;
        }

        public void NetwDone()
        {
            netw_iter++;
        }

        public void Reset(bool listening)
        {
            disk_iter = 0;
            netw_iter = 0;
            this.listening = listening;
            this.finished = false;
        }
    }

    public partial class Main : Form
    {

        private NetListener server;
        private List<NetClient> clients = new List<NetClient>();

        private Thread thread_emitter, thread_listener, thread_disk;

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

                            using (var buffer = new BufferedStream(stream))
                            {
                                ListenToFiles(buffer, files);
                            }

                            progresser.Done();
                        }

                        Invoke(new Action(f.ToClipboard));

                        Thread.Sleep(100);

                        Log(NotificationType.Received, f);
                    }

                    catch (Exception ex) { Log(NotificationType.FailedReceive, ex.Message); continue; }

                    stream.Close();
                }

                Thread.Sleep(50);

                clip_freeze = false;
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

                                using (var buffer = new BufferedStream(stream))
                                {
                                    EmitFiles(buffer);
                                }
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

        private void ListenToFiles(BufferedStream stream, FileBuffer files)
        {
            packets.Reset(true);
            disk_files = files;
            disk_awoker.Set();

            while (!packets.finished)
            {
                var buffer = packets.GetForNetw();
                disk_count = stream.Read(buffer, 0, buffer.Length);
                packets.NetwDone();
            }
        }

        private void EmitFiles(BufferedStream stream)
        {
            packets.Reset(false);
            disk_awoker.Set();

            while (!packets.finished)
            {
                stream.Write(packets.GetForNetw(), 0, disk_count);
                packets.NetwDone();
            }
        }

        MultipacketSwap packets = new MultipacketSwap();
        EventWaitHandle disk_awoker = new EventWaitHandle(false, EventResetMode.AutoReset);
        int disk_count;
        FileBuffer disk_files;

        void ThreadDiskIO()
        {
            while (true)
            {
                disk_awoker.WaitOne();

                byte[] buffer;
                if (!packets.listening)
                    foreach (var file in (clip_data as FileBuffer).files)
                        using (var io = File.OpenRead(file.source))
                        {
                            long size = 0;
                            while (size < file.size && (disk_count = io.Read(buffer = packets.GetForDisk(), 
                                0, buffer.Length)) > 0) { size += disk_count; packets.DiskDone(); }
                        }
                else
                {
                    // Kind little verbose for writing to disks...
                    int iterF = 0; long iterB = 0;

                    foreach (var file in disk_files.files)
                    {
                        progresser.Update(iterF++, file.name);

                        using (var io = File.Create(file.destination))
                        {
                            long size = 0;
                            buffer = packets.GetForDisk();
                            while (size < file.size && disk_count > 0)
                            {
                                io.Write(buffer, 0, disk_count);
                                size += disk_count;
                                progresser.Update(iterB += disk_count);
                                packets.DiskDone();
                                buffer = packets.GetForDisk();
                            }
                        }

                        File.SetLastWriteTime(file.destination, file.modified);
                    }
                }

                packets.finished = true;
            }
        }

        void StartThreader()
        {
            (thread_emitter = new Thread(ThreadEmitter) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
            (thread_listener = new Thread(ThreadListener) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
            (thread_disk = new Thread(ThreadDiskIO) { IsBackground = true, Priority = ThreadPriority.Lowest }).Start();
        }

        void StartServer()
        {
            if (server != null) { server.Stop(); server.Server.Dispose(); }

            var ip = Utility.GetLocalIPAddress();

            (server = ip != null ? new NetListener(ip, config.Port) : null)?.Start();
        }

    }
}
