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

        private NetListener server;
        private List<NetClient> clients = new List<NetClient>();
        private Dictionary<string, string> logs = new Dictionary<string, string>();

        private Thread thread_emitter;
        private Thread thread_listener;
        
        private ManualResetEvent manual_emitter;
        private ManualResetEvent manual_listener;
        
        void ThreadListener()
        {
            while (true)
            {
                if (server.Active && server.Pending())
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

                            Invoke(new Action(() =>
                            {

                                if (f.Type != DataType.MetaInvitation)
                                {
                                    f.SendToClipboard();
                                }
                                else
                                {
                                    var v = f.Data as InvitationBuffer;
                                    if (Inviter.current == null)
                                    {
                                        if (clients.FindIndex(x => x.Name == v.host) < 0)
                                        {
                                            if (MessageBox.Show("Do you trust " + v.host + " so we can add them our list?", "SocketClipboard invitation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                                            {
                                                AddHost(v.host);
                                                SaveConfig();
                                            }
                                        }
                                        Inviter.RespondTo(v, port);
                                    }
                                    else
                                    {
                                        if (clients.FindIndex(x => x.Name == v.host) < 0)
                                        {
                                            AddHost(v.host);
                                            SaveConfig();
                                        }
                                        Inviter.current.IncrementConfirmation();
                                    }
                                }

                            }));


                        }

                        Log(NotificationType.Received, f);
                        g.Close();
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
