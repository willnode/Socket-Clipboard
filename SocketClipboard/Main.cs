using Microsoft.Win32;
using System;
using System.Windows.Forms;
using System.Net.NetworkInformation;
using System.Linq;
using System.Reflection;
using System.Net;

namespace SocketClipboard
{
    public partial class Main : Form
    {
        public SocketConfig config;
        public Progresser progresser;
        public static Main current { get; private set; }
        // private int port = 5000;
        private bool clip_dirty = false;
        private bool clip_freeze = false;
        private ClipBuffer clip_data;

        public Main() : this(false)
        {
        }

        public Main(bool minimized)
        {
            current = this;
            progresser = new Progresser(this);
            config = new SocketConfig();

            InitializeComponent();
            ShowLocalHost();
            StartServer();
            config.Init(this);


            ClipEvent.ClipboardUpdate += ClipboardUpdated;
            NetworkChange.NetworkAddressChanged += IPUpdated;

            StartThreader();

            Log("Software started with param: " + string.Join(" ", Environment.GetCommandLineArgs()));

            Log(NotificationType.Startup, minimized);

        }

        void ShowLocalHost()
        {
            var name = SystemInformation.ComputerName;
            lvMain.Items.Add(name).SubItems.Add("(localhost)");
        }

        void ClipboardUpdated(object sender, EventArgs args)
        {
            if (!clip_freeze && config.Active && !config.Offline)
                clip_dirty = (clip_data = ClipBuffer.FromClipboard()) != null;
        }

        void IPUpdated(object sender, EventArgs args)
        {
            StartServer();
            if (server != null)
                Log(NotificationType.IPUpdated, server.LocalEndpoint);
            else
                Log(NotificationType.IPOffline, null);
            config.Validate();
        }

        RegistryKey GetRegPath()
        {
            RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

            key.CreateSubKey("SocketClipboard");
            return key.OpenSubKey("SocketClipboard", true);
        }

        internal void AddHost(string name)
        {
            lvMain.Items.Add(name).SubItems.Add("...");

            clients.Add(new NetClient(name));

            Log(NotificationType.ClientAdded, name);
        }


        private void _add_Click(object sender, EventArgs e)
        {
            var local = SystemInformation.ComputerName;
            var inviter = new Inviter(clients);
            if (inviter.ShowDialog() == DialogResult.OK)
            {
                foreach (var host in inviter.localhosts)
                {
                    if (host == local) continue;
                    if (clients.All((x) => x.Name != host)) AddHost(host);
                }
                config.Validate();
                config.SaveHosts();
            }

        }

        private void _port_Click(object sender, EventArgs e)
        {
            string __port = config.Port.ToString();
            if (Utility.ShowInputDialog(ref __port, "Enter new IP") == DialogResult.OK)
            {
                var p = int.Parse(__port);
                if (p >= 0 && p <= 65535)
                {
                    config.Port = p;
                    config.Validate();
                    config.SaveConfig();
                    Log(NotificationType.PortUpdated, p);
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
            }
            config.Validate();
            config.SaveHosts();
        }

        private void __clean_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem item in lvMain.Items)
            {
                if (item.Index == 0)
                    continue;

                var tcp = clients[item.Index - 1];
                tcp.Close();
                clients.Remove(tcp);
                item.Remove();
            }
            config.Validate();
            config.SaveHosts();
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
            config.RunAtStartup = __runat.Checked = !__runat.Checked;
        }



        private void __about_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Socket-Clipboard is made with <3 by Wellosoft.\nThis is an open source project. Visit reposity?", "SocketClipboard " +
                AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version.ToSt‌​ring(), MessageBoxButtons.OKCancel) == DialogResult.OK)
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

        //private void __quota0_Click(object sender, EventArgs e)
        //{
        //    limit = (int)((ToolStripMenuItem)sender).Tag;
        //    SaveConfig();
        //}

        //private void __notify0_Click(object sender, EventArgs e)
        //{
        //    verbose = (int)((ToolStripMenuItem)sender).Tag;
        //    SaveConfig();
        //}

        private void Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }

        internal bool ProgresserClosing()
        {
            // TODO: Stop the network operaion
            // return true to reuse
            return true;
        }

        private void ___toggle_Click(object sender, EventArgs e)
        {
            if (this.Visible = !this.Visible)
                config.Validate();
        }

        private void __active_Click(object sender, EventArgs e)
        {
            config.Active = !config.Active;
            config.Validate(true);
        }

        private void __solo_Click(object sender, EventArgs e)
        {
            config.Solo = !config.Solo;
            config.Validate(true);
        }

        private void __mute_Click(object sender, EventArgs e)
        {
            config.Mute = !config.Mute;
            config.Validate(true);
        }

        int soloIndex = -1;

        private void lvMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            soloIndex = lvMain.SelectedIndices.Count == 0 ? -1 : lvMain.SelectedIndices[0] - 1;
            if (config.Solo)
                config.Validate();
        }

        private void __file0_Click(object sender, EventArgs e)
        {
            config.FileTransfer = (FileTransferFlag)(sender as ToolStripMenuItem).Tag;
            config.Validate(true);
        }

        private void __notify0_Click(object sender, EventArgs e)
        {
            config.Notify = (NotifyFlag)(sender as ToolStripMenuItem).Tag;
            config.Validate(true);
        }
    }

}
