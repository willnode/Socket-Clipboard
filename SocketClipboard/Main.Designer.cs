namespace SocketClipboard
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.lvMain = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._port = new System.Windows.Forms.ToolStripStatusLabel();
            this._status = new System.Windows.Forms.ToolStripStatusLabel();
            this._notify = new System.Windows.Forms.NotifyIcon(this.components);
            this._notify_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.___toggle = new System.Windows.Forms.ToolStripMenuItem();
            this.___active = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.___restart = new System.Windows.Forms.ToolStripMenuItem();
            this.___quit = new System.Windows.Forms.ToolStripMenuItem();
            this._split = new System.Windows.Forms.SplitContainer();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.hostsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__add = new System.Windows.Forms.ToolStripMenuItem();
            this.@__rem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__clean = new System.Windows.Forms.ToolStripMenuItem();
            this.connectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__active = new System.Windows.Forms.ToolStripMenuItem();
            this.@__solo = new System.Windows.Forms.ToolStripMenuItem();
            this.@__mute = new System.Windows.Forms.ToolStripMenuItem();
            this.configToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__runat = new System.Windows.Forms.ToolStripMenuItem();
            this.fileTransferToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__file0 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__file1 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__file2 = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify0 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify1 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.@__reset = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__about = new System.Windows.Forms.ToolStripMenuItem();
            this.@__restart = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quit = new System.Windows.Forms.ToolStripMenuItem();
            this._log = new System.Windows.Forms.ListBox();
            this.@__server = new System.Windows.Forms.ToolStripMenuItem();
            this.@__version = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1.SuspendLayout();
            this._notify_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
            this._split.Panel1.SuspendLayout();
            this._split.Panel2.SuspendLayout();
            this._split.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvMain
            // 
            this.lvMain.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvMain.FullRowSelect = true;
            this.lvMain.GridLines = true;
            this.lvMain.Location = new System.Drawing.Point(0, 24);
            this.lvMain.MultiSelect = false;
            this.lvMain.Name = "lvMain";
            this.lvMain.Size = new System.Drawing.Size(402, 226);
            this.lvMain.TabIndex = 0;
            this.lvMain.UseCompatibleStateImageBehavior = false;
            this.lvMain.View = System.Windows.Forms.View.Details;
            this.lvMain.SelectedIndexChanged += new System.EventHandler(this.lvMain_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Host";
            this.columnHeader1.Width = 163;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "State";
            this.columnHeader2.Width = 167;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._port,
            this._status});
            this.statusStrip1.Location = new System.Drawing.Point(0, 250);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(402, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // _port
            // 
            this._port.IsLink = true;
            this._port.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._port.LinkColor = System.Drawing.SystemColors.ControlText;
            this._port.Name = "_port";
            this._port.Size = new System.Drawing.Size(66, 17);
            this._port.Text = "PORT: 5000";
            this._port.Click += new System.EventHandler(this._port_Click);
            // 
            // _status
            // 
            this._status.IsLink = true;
            this._status.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this._status.LinkColor = System.Drawing.SystemColors.ControlText;
            this._status.Name = "_status";
            this._status.Size = new System.Drawing.Size(321, 17);
            this._status.Spring = true;
            this._status.Text = "Ready.";
            this._status.Click += new System.EventHandler(this._status_Click);
            // 
            // _notify
            // 
            this._notify.ContextMenuStrip = this._notify_strip;
            this._notify.Icon = ((System.Drawing.Icon)(resources.GetObject("_notify.Icon")));
            this._notify.Text = "Clipboard-Socket configuration";
            this._notify.Visible = true;
            this._notify.MouseClick += new System.Windows.Forms.MouseEventHandler(this._notify_MouseClick);
            // 
            // _notify_strip
            // 
            this._notify_strip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.___toggle,
            this.___active,
            this.toolStripSeparator1,
            this.___restart,
            this.___quit});
            this._notify_strip.Name = "_notify_strip";
            this._notify_strip.Size = new System.Drawing.Size(128, 98);
            // 
            // ___toggle
            // 
            this.___toggle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.___toggle.Name = "___toggle";
            this.___toggle.Size = new System.Drawing.Size(127, 22);
            this.___toggle.Text = "Toggle UI";
            this.___toggle.Click += new System.EventHandler(this.___toggle_Click);
            // 
            // ___active
            // 
            this.___active.Checked = true;
            this.___active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.___active.Name = "___active";
            this.___active.Size = new System.Drawing.Size(127, 22);
            this.___active.Text = "Active";
            this.___active.Click += new System.EventHandler(this.@__active_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(124, 6);
            // 
            // ___restart
            // 
            this.___restart.Name = "___restart";
            this.___restart.Size = new System.Drawing.Size(127, 22);
            this.___restart.Text = "Restart";
            this.___restart.Click += new System.EventHandler(this.@__restart_Click);
            // 
            // ___quit
            // 
            this.___quit.Name = "___quit";
            this.___quit.Size = new System.Drawing.Size(127, 22);
            this.___quit.Text = "Quit";
            this.___quit.Click += new System.EventHandler(this.@__quit_Click);
            // 
            // _split
            // 
            this._split.Dock = System.Windows.Forms.DockStyle.Fill;
            this._split.Location = new System.Drawing.Point(0, 0);
            this._split.Name = "_split";
            // 
            // _split.Panel1
            // 
            this._split.Panel1.Controls.Add(this.lvMain);
            this._split.Panel1.Controls.Add(this.menuStrip1);
            this._split.Panel1MinSize = 100;
            // 
            // _split.Panel2
            // 
            this._split.Panel2.Controls.Add(this._log);
            this._split.Panel2Collapsed = true;
            this._split.Panel2MinSize = 150;
            this._split.Size = new System.Drawing.Size(402, 250);
            this._split.SplitterDistance = 100;
            this._split.TabIndex = 3;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hostsToolStripMenuItem,
            this.connectionToolStripMenuItem,
            this.configToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(402, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // hostsToolStripMenuItem
            // 
            this.hostsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__add,
            this.@__rem,
            this.@__clean});
            this.hostsToolStripMenuItem.Name = "hostsToolStripMenuItem";
            this.hostsToolStripMenuItem.Size = new System.Drawing.Size(49, 20);
            this.hostsToolStripMenuItem.Text = "Hosts";
            // 
            // __add
            // 
            this.@__add.Name = "__add";
            this.@__add.ShortcutKeyDisplayString = "Insert";
            this.@__add.ShortcutKeys = System.Windows.Forms.Keys.Insert;
            this.@__add.Size = new System.Drawing.Size(167, 22);
            this.@__add.Text = "Add";
            this.@__add.ToolTipText = "Add hosts to the list";
            this.@__add.Click += new System.EventHandler(this._add_Click);
            // 
            // __rem
            // 
            this.@__rem.Name = "__rem";
            this.@__rem.ShortcutKeyDisplayString = "Delete";
            this.@__rem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.@__rem.Size = new System.Drawing.Size(167, 22);
            this.@__rem.Text = "Remove";
            this.@__rem.ToolTipText = "Remove selected hosts";
            this.@__rem.Click += new System.EventHandler(this._remove_Click);
            // 
            // __clean
            // 
            this.@__clean.Name = "__clean";
            this.@__clean.ShortcutKeyDisplayString = "Alt+Delete";
            this.@__clean.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.Delete)));
            this.@__clean.Size = new System.Drawing.Size(167, 22);
            this.@__clean.Text = "Clean";
            this.@__clean.ToolTipText = "Remove All hosts";
            this.@__clean.Click += new System.EventHandler(this.@__clean_Click);
            // 
            // connectionToolStripMenuItem
            // 
            this.connectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__active,
            this.@__solo,
            this.@__mute});
            this.connectionToolStripMenuItem.Name = "connectionToolStripMenuItem";
            this.connectionToolStripMenuItem.Size = new System.Drawing.Size(72, 20);
            this.connectionToolStripMenuItem.Text = "Behaviour";
            // 
            // __active
            // 
            this.@__active.Name = "__active";
            this.@__active.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.J)));
            this.@__active.Size = new System.Drawing.Size(152, 22);
            this.@__active.Text = "Active";
            this.@__active.ToolTipText = "Make socket actively watch clipboard activity";
            this.@__active.Click += new System.EventHandler(this.@__active_Click);
            // 
            // __solo
            // 
            this.@__solo.Name = "__solo";
            this.@__solo.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.K)));
            this.@__solo.Size = new System.Drawing.Size(152, 22);
            this.@__solo.Text = "Solo";
            this.@__solo.ToolTipText = "Focus packet sending to only one host which selected in list";
            this.@__solo.Click += new System.EventHandler(this.@__solo_Click);
            // 
            // __mute
            // 
            this.@__mute.Name = "__mute";
            this.@__mute.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.M)));
            this.@__mute.Size = new System.Drawing.Size(152, 22);
            this.@__mute.Text = "Mute";
            this.@__mute.ToolTipText = "Don\'t receive packets";
            this.@__mute.Click += new System.EventHandler(this.@__mute_Click);
            // 
            // configToolStripMenuItem
            // 
            this.configToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__runat,
            this.fileTransferToolStripMenuItem,
            this.notificationToolStripMenuItem1,
            this.toolStripSeparator2,
            this.@__reset,
            this.@__server});
            this.configToolStripMenuItem.Name = "configToolStripMenuItem";
            this.configToolStripMenuItem.Size = new System.Drawing.Size(55, 20);
            this.configToolStripMenuItem.Text = "Config";
            // 
            // __runat
            // 
            this.@__runat.Name = "__runat";
            this.@__runat.Size = new System.Drawing.Size(152, 22);
            this.@__runat.Text = "Run at Startup";
            this.@__runat.ToolTipText = "Allow this software to run as windows startup";
            this.@__runat.Click += new System.EventHandler(this.@__startup_Click);
            // 
            // fileTransferToolStripMenuItem
            // 
            this.fileTransferToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__file0,
            this.@__file1,
            this.@__file2});
            this.fileTransferToolStripMenuItem.Name = "fileTransferToolStripMenuItem";
            this.fileTransferToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.fileTransferToolStripMenuItem.Text = "File Transfer";
            this.fileTransferToolStripMenuItem.ToolTipText = "Outgoing file policy";
            // 
            // __file0
            // 
            this.@__file0.Name = "__file0";
            this.@__file0.Size = new System.Drawing.Size(155, 22);
            this.@__file0.Text = "Block";
            this.@__file0.ToolTipText = "Don\'t send files";
            this.@__file0.Click += new System.EventHandler(this.@__file0_Click);
            // 
            // __file1
            // 
            this.@__file1.Name = "__file1";
            this.@__file1.Size = new System.Drawing.Size(155, 22);
            this.@__file1.Text = "Single File Only";
            this.@__file1.ToolTipText = "Only if I copy a single file";
            this.@__file1.Click += new System.EventHandler(this.@__file0_Click);
            // 
            // __file2
            // 
            this.@__file2.Name = "__file2";
            this.@__file2.Size = new System.Drawing.Size(155, 22);
            this.@__file2.Text = "Allow";
            this.@__file2.ToolTipText = "Send always";
            this.@__file2.Click += new System.EventHandler(this.@__file0_Click);
            // 
            // notificationToolStripMenuItem1
            // 
            this.notificationToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__notify0,
            this.@__notify1,
            this.@__notify2});
            this.notificationToolStripMenuItem1.Name = "notificationToolStripMenuItem1";
            this.notificationToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.notificationToolStripMenuItem1.Text = "Notification";
            this.notificationToolStripMenuItem1.ToolTipText = "Notification policy";
            // 
            // __notify0
            // 
            this.@__notify0.Name = "__notify0";
            this.@__notify0.Size = new System.Drawing.Size(152, 22);
            this.@__notify0.Text = "Silent";
            this.@__notify0.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // __notify1
            // 
            this.@__notify1.Name = "__notify1";
            this.@__notify1.Size = new System.Drawing.Size(152, 22);
            this.@__notify1.Text = "Informative";
            this.@__notify1.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // __notify2
            // 
            this.@__notify2.Name = "__notify2";
            this.@__notify2.Size = new System.Drawing.Size(152, 22);
            this.@__notify2.Text = "Verbose";
            this.@__notify2.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(149, 6);
            // 
            // __reset
            // 
            this.@__reset.Name = "__reset";
            this.@__reset.Size = new System.Drawing.Size(152, 22);
            this.@__reset.Text = "Reset";
            this.@__reset.ToolTipText = "Reset configuration to default";
            this.@__reset.Click += new System.EventHandler(this.@__reset_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__version,
            this.@__about,
            this.@__restart,
            this.@__quit});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // __about
            // 
            this.@__about.Name = "__about";
            this.@__about.Size = new System.Drawing.Size(152, 22);
            this.@__about.Text = "About";
            this.@__about.ToolTipText = "If you curious ...";
            this.@__about.Click += new System.EventHandler(this.@__about_Click);
            // 
            // __restart
            // 
            this.@__restart.Name = "__restart";
            this.@__restart.Size = new System.Drawing.Size(152, 22);
            this.@__restart.Text = "Restart";
            this.@__restart.ToolTipText = "Restart the software";
            this.@__restart.Click += new System.EventHandler(this.@__restart_Click);
            // 
            // __quit
            // 
            this.@__quit.Name = "__quit";
            this.@__quit.Size = new System.Drawing.Size(152, 22);
            this.@__quit.Text = "Quit";
            this.@__quit.ToolTipText = "Kill the software";
            this.@__quit.Click += new System.EventHandler(this.@__quit_Click);
            // 
            // _log
            // 
            this._log.Dock = System.Windows.Forms.DockStyle.Fill;
            this._log.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._log.FormattingEnabled = true;
            this._log.ItemHeight = 14;
            this._log.Location = new System.Drawing.Point(0, 0);
            this._log.Name = "_log";
            this._log.ScrollAlwaysVisible = true;
            this._log.Size = new System.Drawing.Size(96, 100);
            this._log.TabIndex = 0;
            // 
            // __server
            // 
            this.@__server.CheckOnClick = true;
            this.@__server.Name = "__server";
            this.@__server.Size = new System.Drawing.Size(152, 22);
            this.@__server.Text = "Run Server";
            this.@__server.ToolTipText = "Run copyserver so that your neighbor can download a copy of socket-clipboard dire" +
    "ctly if they haven\'t";
            this.@__server.Click += new System.EventHandler(this.@__server_Click);
            // 
            // __version
            // 
            this.@__version.Enabled = false;
            this.@__version.Name = "__version";
            this.@__version.Size = new System.Drawing.Size(152, 22);
            this.@__version.Text = "v1.3.0.0";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 272);
            this.Controls.Add(this._split);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "Main";
            this.Text = "Socket-Clipboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._notify_strip.ResumeLayout(false);
            this._split.Panel1.ResumeLayout(false);
            this._split.Panel1.PerformLayout();
            this._split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
            this._split.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _port;
        private System.Windows.Forms.ToolStripStatusLabel _status;
        private System.Windows.Forms.NotifyIcon _notify;
        private System.Windows.Forms.SplitContainer _split;
        private System.Windows.Forms.ListBox _log;
        private System.Windows.Forms.ContextMenuStrip _notify_strip;
        private System.Windows.Forms.ToolStripMenuItem ___toggle;
        private System.Windows.Forms.ToolStripMenuItem ___active;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem ___restart;
        private System.Windows.Forms.ToolStripMenuItem ___quit;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem hostsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __add;
        private System.Windows.Forms.ToolStripMenuItem __rem;
        private System.Windows.Forms.ToolStripMenuItem __clean;
        private System.Windows.Forms.ToolStripMenuItem connectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __active;
        private System.Windows.Forms.ToolStripMenuItem __solo;
        private System.Windows.Forms.ToolStripMenuItem __mute;
        private System.Windows.Forms.ToolStripMenuItem configToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __runat;
        private System.Windows.Forms.ToolStripMenuItem fileTransferToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __file0;
        private System.Windows.Forms.ToolStripMenuItem __file1;
        private System.Windows.Forms.ToolStripMenuItem __file2;
        private System.Windows.Forms.ToolStripMenuItem notificationToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem __notify0;
        private System.Windows.Forms.ToolStripMenuItem __notify1;
        private System.Windows.Forms.ToolStripMenuItem __notify2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem __reset;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __about;
        private System.Windows.Forms.ToolStripMenuItem __quit;
        private System.Windows.Forms.ToolStripMenuItem __restart;
        private System.Windows.Forms.ToolStripMenuItem __server;
        private System.Windows.Forms.ToolStripMenuItem __version;
    }
}

