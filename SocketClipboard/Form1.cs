using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;

namespace SocketClipboard
{
    using Resources = SocketClipboard.Properties.Resources;
    public partial class Form1 : Form
    {

        private int port = 5000;
        private bool autorun = false;
        private bool clip_dirty = false;
        private bool clip_freeze = false;
        private ClipData clip_data;

        private TcpListener server;
        private List<NetClient> clients = new List<NetClient>();
        // 0: Okay, 1: Can't send, 2: Unknown
        private Dictionary<string, int> logs = new Dictionary<string, int>();

        private Thread thread_emitter;
        private Thread thread_listener;

        public Form1()
        {
            InitializeComponent();
            ShowLocalHost();
            ReadConfig();
            ClipEvent.ClipboardUpdate += ClipboardUpdated;
            StartServer();
            StartThreader();

            _notify.ShowBalloonTip(3000, "Socket-Clipboard is online", "Don't forget to run this software and list all your computer clients", ToolTipIcon.Info);
        }

        void ShowLocalHost ()
        {
            var name = SystemInformation.ComputerName;
            lvMain.Items.Add(name).SubItems.Add("(localhost)");
        }

        void ReadConfig ()
        {
            var reg = GetRegPath();
            port = (int)reg.GetValue("PORT", port);
            autorun = (bool)reg.GetValue("AUTORUN", false);
            var hosts2 = (reg.GetValue("HOSTS") as string);
            if (hosts2 != null)
            {
                var hosts = hosts2.Split('|');
                foreach (var host in hosts)
                {
                    AddHost(host);
                }
            }
        }

        void SaveConfig ()
        {
            var reg = GetRegPath();
            reg.SetValue("PORT", port);
            reg.SetValue("HOSTS", string.Join("|", clients.ConvertAll(x => x.Name)));
        }

        void ClipboardUpdated (object sender, EventArgs args)
        {
            if (clip_freeze)
            {
                return;
            }

            clip_dirty = (clip_data = ClipData.FromClipboard()) != null;
            
        }

        void StartThreader ()
        {
            thread_emitter = new Thread(ThreadEmitter);
            thread_emitter.IsBackground = true;
            thread_emitter.Start();

            thread_listener = new Thread(ThreadListener);
            thread_listener.IsBackground = true;
            thread_listener.Start();
        }

        void StartServer ()
        {
            if (server != null)
                server.Stop();

            server = new TcpListener(Utility.GetLocalIPAddress(), port);
            server.Start();
        }

        void ThreadListener ()
        {
            while (true)
            {
                if (server.Pending())
                {
                    var tcp = server.AcceptTcpClient();
                    Log("Receiving...", StateLog.Listen);
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

                        Log(string.Format("Received {0} ({1})", f.Type.ToString(), f.GetSizeReadable()), StateLog.Normal);
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

                Log("Emitting ...", StateLog.Busy);

                for (int i = 0; i < clients.Count; i++)
                {
                    var tcp = clients[i];
                    var ok = true;
                    try
                    {
                        ok = (tcp.ConnectAsync(tcp.Name, port).Wait(1000));
                    }
                    catch (Exception)
                    {
                        ok = false;
                    }
                    logs[tcp.Name] = ok ? 0 : 1;
                    if (ok)
                    {
                        var g = tcp.GetStream();
                        {
                            new BinaryFormatter().Serialize(g, clip_data);
                        }
                        g.Close();
                    }

                    // Close and set the new one
                    tcp.Close();
                    clients[i] = new NetClient(tcp.Name);
                }

                Log(string.Format("Emitted {0} ({1})", clip_data.Type.ToString(), clip_data.GetSizeReadable()), StateLog.Normal);

            }
        }

        RegistryKey GetRegPath()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            key.CreateSubKey("SocketClipboard");
            return key.OpenSubKey("SocketClipboard", true);
        }

        void AddHost (string name)
        {
            var item = lvMain.Items.Add(name);
            item.SubItems.Add("...");

            var client = new NetClient(name);
            clients.Add(client);

            Log("Added " + name);
        }


        private void _add_Click(object sender, EventArgs e)
        {
            string client = "";
            if(Utility.ShowInputDialog(ref client, "Enter Computer Name") == DialogResult.OK && !string.IsNullOrEmpty(client))
            {
                AddHost(client);
                SaveConfig();
            }

        }

        private void Log (string text, bool timestamp = true)
        {
            if (!this.Visible)
                return;
            if (timestamp)
            {
                text = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ff"), text);
            }
            Console.WriteLine(text);
            Invoke(new Action (() => { _status.Text = text; UpdateLabels(); }));
        }

        private void Log(string text, StateLog state)
        {
            Log(text);
            LogState(state);
        }

        enum StateLog
        {
            Normal = 0,
            Busy = 1,
            Listen = 2,
        }
        private void LogState (StateLog state)
        {
            Invoke(new Action(() =>
            {
                switch (state)
                {
                    case StateLog.Normal:
                        _notify.Icon = Resources.state_normal;
                        break;
                    case StateLog.Busy:
                        _notify.Icon = Resources.state_busy;
                        break;
                    case StateLog.Listen:
                        _notify.Icon = Resources.state_listen;
                        break;
                }
            }));
        }

        void UpdateLabels ()
        {
            foreach (ListViewItem item in lvMain.Items)
            {
                if (item.Index == 0)
                    continue;
                var tcp = clients[item.Index - 1];
                int log;
                if(!logs.TryGetValue(tcp.Name, out log))
                    item.SubItems[1].Text = "...";
                else
                    item.SubItems[1].Text = log == 0 ? "Connected" : "Disconnected";
            }
        }

        private void _port_Click(object sender, EventArgs e)
        {
            string __port = port.ToString();
            if (Utility.ShowInputDialog(ref __port, "Enter new IP") == DialogResult.OK)
            {
                var p = int.Parse(__port);
                if (p >= 0 && p <= 65535)
                {
                    port = p;
                    _port.Text = "PORT: " + port.ToString();
                    SaveConfig();
                }
            }
        }

        private void _remove_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvMain.SelectedItems)
            {
                if (item.Index == 0)
                    continue;

                var tcp = clients[item.Index - 1];
                tcp.Close();
                clients.Remove(tcp);
                item.Remove();
                SaveConfig();
            }
        }

      
        private void _notify_MouseClick(object sender, MouseEventArgs e)
        {
            this.Visible = !this.Visible;
        }
    }
}
