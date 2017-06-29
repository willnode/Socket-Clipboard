using Microsoft.Win32;
using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;

namespace SocketClipboard
{
    public partial class Main : Form
    {

        public static Main current { get; private set; }
        private int port = 5000;
        private bool clip_dirty = false;
        private bool clip_freeze = false;
        private ClipData clip_data;

        // 0 = off, 1 = normal, 2 = verbose
        public static int verbose = 1;
        // 0 = 1 MB, 1 = 50 MB, 2 = 1 GB, 3 = Unlimited
        public static int limit = 1;

        public Main() : this(false)
        {
        }

        public Main (bool minimized)
        {
            current = this;

            InitializeComponent();
            ShowLocalHost();
            ReadConfig();

            ClipEvent.ClipboardUpdate += ClipboardUpdated;
            NetworkChange.NetworkAddressChanged += IPUpdated;

            StartServer();
            StartThreader();

            Log("Software started with param: " + string.Join(" ", Environment.GetCommandLineArgs()));

            Log(NotificationType.Startup, minimized);
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
            verbose = (int)reg.GetValue("VERBOSE", verbose);
            limit = (int)reg.GetValue("LIMIT", limit);
            var hosts2 = (reg.GetValue("HOSTS") as string);
            if (hosts2 != null)
            {
                var hosts = hosts2.Split('|');
                foreach (var host in hosts)
                {
                    AddHost(host);
                }
            }

            // Update UI

            _port.Text = "PORT: " + port.ToString();
            __notify0.Checked = verbose == (int)__notify0.Tag;
            __notify1.Checked = verbose == (int)__notify1.Tag;
            __notify2.Checked = verbose == (int)__notify2.Tag;

            __quota0.Checked = limit == (int)__quota0.Tag;
            __quota1.Checked = limit == (int)__quota1.Tag;
            __quota2.Checked = limit == (int)__quota2.Tag;
            __quota3.Checked = limit == (int)__quota3.Tag;
            __startup.Checked = Startup;
        }

        void SaveConfig ()
        {
            var reg = GetRegPath();
            reg.SetValue("PORT", port);
            reg.SetValue("VERBOSE", verbose);
            reg.SetValue("LIMIT", limit);
            reg.SetValue("HOSTS", string.Join("|", clients.ConvertAll(x => x.Name)));
        }

        void ClipboardUpdated (object sender, EventArgs args)
        {
            if (!clip_freeze)
                clip_dirty = (clip_data = ClipData.FromClipboard()) != null;
        }

        void IPUpdated (object sender, EventArgs args)
        {
            StartServer();
            Log(NotificationType.IPUpdated, server.LocalEndpoint);
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

            Log(NotificationType.ClientAdded, name);
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
                    Log(NotificationType.PortUpdated, port);
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
            if (e.Button == MouseButtons.Left)
                this.Visible = !this.Visible;
        }

        private void _status_Click(object sender, EventArgs e)
        {
            _split.Panel2Collapsed = !_split.Panel2Collapsed;
        }

        private void __startup_Click(object sender, EventArgs e)
        {
            Startup = __startup.Checked = !__startup.Checked;
        }

        private bool Startup
        {
            get
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                   ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                return (string)rk.GetValue("Socket-Clipboard", null) == Application.ExecutablePath + " --background";
            }
            set
            {
                RegistryKey rk = Registry.CurrentUser.OpenSubKey
                    ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                if (value)
                    rk.SetValue("Socket-Clipboard", Application.ExecutablePath + " --background");
                else
                    rk.DeleteValue("Socket-Clipboard", false);
            }

        }

        private void __about_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Socket-Clipboard is made with <3 by Wellosoft.\nThis is an open source project. Visit reposity?", "About SocketClipboard", MessageBoxButtons.YesNo) == DialogResult.Yes)
                System.Diagnostics.Process.Start("https://github.com/willnode/Socket-Clipboard/");
        }

        private void __restart_Click(object sender, EventArgs e)
        {
            Application.Restart();
            Environment.Exit(0);
        }

        private void __quit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void __quota0_Click(object sender, EventArgs e)
        {
            limit = (int)((ToolStripMenuItem)sender).Tag;
            SaveConfig();
        }

        private void __notify0_Click(object sender, EventArgs e)
        {
            verbose = (int)((ToolStripMenuItem)sender).Tag;
            SaveConfig();
        }

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
