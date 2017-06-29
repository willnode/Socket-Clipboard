using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketClipboard
{
    public partial class Main : Form
    {

        private TcpListener server;
        private List<NetClient> clients = new List<NetClient>();
        private Dictionary<string, string> logs = new Dictionary<string, string>();

        private Thread thread_emitter;
        private Thread thread_listener;


        void ThreadListener()
        {
            while (true)
            {
                if (server.Pending())
                {
                    Log(NotificationType.Receiving);

                    var tcp = server.AcceptTcpClient();
                    var b = new BinaryFormatter();
                    var g = tcp.GetStream();
                    {
                        ClipData f = null;
                        try
                        {
                            f = b.Deserialize(g) as ClipData;
                        }
                        catch (Exception ex)
                        {
                            Log(ex.Message);
                            continue;
                        }

                        if (f != null)
                        {
                            // Clip Freeze prevents the next copy buffer event makes problem.
                            clip_freeze = true;

                            Invoke(new Action(() => {
                                f.SendToClipboard();
                            }));

                        }

                        Log(NotificationType.Received, f);
                        g.Close();
                    }
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
                    try
                    {
                        var task = (tcp.ConnectAsync(tcp.Name, port));
                        ok = task.Wait(1000);
                        logs[tcp.Name] = ok ? "Sent" : "Failed";
                    }
                    catch (Exception ex)
                    {
                        ok = false;
                        logs[tcp.Name] = "Failed: " + (ex.InnerException == null ? ex.Message : ex.InnerException.Message);
                    }
                    if (ok)
                    {
                        var g = tcp.GetStream();
                        new BinaryFormatter().Serialize(g, clip_data);
                        g.Close();
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
            thread_emitter = new Thread(ThreadEmitter);
            thread_emitter.IsBackground = true;
            thread_emitter.Start();

            thread_listener = new Thread(ThreadListener);
            thread_listener.IsBackground = true;
            thread_listener.Start();
        }

        void StartServer()
        {
            if (server != null)
                server.Stop();

            server = new TcpListener(Utility.GetLocalIPAddress(), port);
            server.Start();
        }

    }
}
