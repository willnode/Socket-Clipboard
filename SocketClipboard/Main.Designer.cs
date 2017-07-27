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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this._add = new System.Windows.Forms.Button();
            this._remove = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this._port = new System.Windows.Forms.ToolStripStatusLabel();
            this._status = new System.Windows.Forms.ToolStripStatusLabel();
            this._notify = new System.Windows.Forms.NotifyIcon(this.components);
            this._notify_strip = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.@__active = new System.Windows.Forms.ToolStripMenuItem();
            this.@__startup = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.fileTransferLimitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quota0 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quota1 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quota2 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quota3 = new System.Windows.Forms.ToolStripMenuItem();
            this.notificationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify0 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify1 = new System.Windows.Forms.ToolStripMenuItem();
            this.@__notify2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.@__about = new System.Windows.Forms.ToolStripMenuItem();
            this.@__restart = new System.Windows.Forms.ToolStripMenuItem();
            this.@__quit = new System.Windows.Forms.ToolStripMenuItem();
            this._split = new System.Windows.Forms.SplitContainer();
            this._log = new System.Windows.Forms.ListBox();
            this.@__invite = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this._notify_strip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._split)).BeginInit();
            this._split.Panel1.SuspendLayout();
            this._split.Panel2.SuspendLayout();
            this._split.SuspendLayout();
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
            this.lvMain.Location = new System.Drawing.Point(0, 40);
            this.lvMain.MultiSelect = false;
            this.lvMain.Name = "lvMain";
            this.lvMain.Size = new System.Drawing.Size(402, 210);
            this.lvMain.TabIndex = 0;
            this.lvMain.UseCompatibleStateImageBehavior = false;
            this.lvMain.View = System.Windows.Forms.View.Details;
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
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this._add, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this._remove, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(402, 40);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // _add
            // 
            this._add.Dock = System.Windows.Forms.DockStyle.Fill;
            this._add.Location = new System.Drawing.Point(3, 3);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(195, 34);
            this._add.TabIndex = 0;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _remove
            // 
            this._remove.Dock = System.Windows.Forms.DockStyle.Fill;
            this._remove.Location = new System.Drawing.Point(204, 3);
            this._remove.Name = "_remove";
            this._remove.Size = new System.Drawing.Size(195, 34);
            this._remove.TabIndex = 1;
            this._remove.Text = "Remove";
            this._remove.UseVisualStyleBackColor = true;
            this._remove.Click += new System.EventHandler(this._remove_Click);
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
            this.@__active,
            this.@__startup,
            this.toolStripSeparator1,
            this.@__invite,
            this.fileTransferLimitToolStripMenuItem,
            this.notificationToolStripMenuItem,
            this.toolStripSeparator2,
            this.@__about,
            this.@__restart,
            this.@__quit});
            this._notify_strip.Name = "_notify_strip";
            this._notify_strip.Size = new System.Drawing.Size(168, 214);
            // 
            // __active
            // 
            this.@__active.Checked = true;
            this.@__active.CheckState = System.Windows.Forms.CheckState.Checked;
            this.@__active.Name = "__active";
            this.@__active.Size = new System.Drawing.Size(167, 22);
            this.@__active.Text = "Active";
            this.@__active.Click += new System.EventHandler(this.@__active_Click);
            // 
            // __startup
            // 
            this.@__startup.Name = "__startup";
            this.@__startup.Size = new System.Drawing.Size(167, 22);
            this.@__startup.Text = "Run at Startup";
            this.@__startup.Click += new System.EventHandler(this.@__startup_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(164, 6);
            // 
            // fileTransferLimitToolStripMenuItem
            // 
            this.fileTransferLimitToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__quota0,
            this.@__quota1,
            this.@__quota2,
            this.@__quota3});
            this.fileTransferLimitToolStripMenuItem.Name = "fileTransferLimitToolStripMenuItem";
            this.fileTransferLimitToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.fileTransferLimitToolStripMenuItem.Text = "File Transfer Limit";
            // 
            // __quota0
            // 
            this.@__quota0.Name = "__quota0";
            this.@__quota0.Size = new System.Drawing.Size(192, 22);
            this.@__quota0.Tag = 0;
            this.@__quota0.Text = "1 MB";
            this.@__quota0.Click += new System.EventHandler(this.@__quota0_Click);
            // 
            // __quota1
            // 
            this.@__quota1.Name = "__quota1";
            this.@__quota1.Size = new System.Drawing.Size(192, 22);
            this.@__quota1.Tag = 1;
            this.@__quota1.Text = "50 MB";
            this.@__quota1.Click += new System.EventHandler(this.@__quota0_Click);
            // 
            // __quota2
            // 
            this.@__quota2.Name = "__quota2";
            this.@__quota2.Size = new System.Drawing.Size(192, 22);
            this.@__quota2.Tag = 2;
            this.@__quota2.Text = "1 GB";
            this.@__quota2.Click += new System.EventHandler(this.@__quota0_Click);
            // 
            // __quota3
            // 
            this.@__quota3.Name = "__quota3";
            this.@__quota3.Size = new System.Drawing.Size(192, 22);
            this.@__quota3.Tag = 3;
            this.@__quota3.Text = "Unlimited (Be careful!)";
            this.@__quota3.Click += new System.EventHandler(this.@__quota0_Click);
            // 
            // notificationToolStripMenuItem
            // 
            this.notificationToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.@__notify0,
            this.@__notify1,
            this.@__notify2});
            this.notificationToolStripMenuItem.Name = "notificationToolStripMenuItem";
            this.notificationToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.notificationToolStripMenuItem.Text = "Notification";
            // 
            // __notify0
            // 
            this.@__notify0.Name = "__notify0";
            this.@__notify0.Size = new System.Drawing.Size(115, 22);
            this.@__notify0.Tag = 0;
            this.@__notify0.Text = "Off";
            this.@__notify0.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // __notify1
            // 
            this.@__notify1.Name = "__notify1";
            this.@__notify1.Size = new System.Drawing.Size(115, 22);
            this.@__notify1.Tag = 1;
            this.@__notify1.Text = "Basic";
            this.@__notify1.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // __notify2
            // 
            this.@__notify2.Name = "__notify2";
            this.@__notify2.Size = new System.Drawing.Size(115, 22);
            this.@__notify2.Tag = 2;
            this.@__notify2.Text = "Verbose";
            this.@__notify2.Click += new System.EventHandler(this.@__notify0_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(164, 6);
            // 
            // __about
            // 
            this.@__about.Name = "__about";
            this.@__about.Size = new System.Drawing.Size(167, 22);
            this.@__about.Text = "About";
            this.@__about.Click += new System.EventHandler(this.@__about_Click);
            // 
            // __restart
            // 
            this.@__restart.Name = "__restart";
            this.@__restart.Size = new System.Drawing.Size(167, 22);
            this.@__restart.Text = "Restart";
            this.@__restart.Click += new System.EventHandler(this.@__restart_Click);
            // 
            // __quit
            // 
            this.@__quit.Name = "__quit";
            this.@__quit.Size = new System.Drawing.Size(167, 22);
            this.@__quit.Text = "Quit";
            this.@__quit.Click += new System.EventHandler(this.@__quit_Click);
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
            this._split.Panel1.Controls.Add(this.tableLayoutPanel1);
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
            // __invite
            // 
            this.@__invite.Name = "__invite";
            this.@__invite.Size = new System.Drawing.Size(167, 22);
            this.@__invite.Text = "Send Invitations...";
            this.@__invite.Click += new System.EventHandler(this.@__invite_Click);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(402, 272);
            this.Controls.Add(this._split);
            this.Controls.Add(this.statusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(250, 200);
            this.Name = "Main";
            this.Text = "Socket-Clipboard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Main_FormClosing);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this._notify_strip.ResumeLayout(false);
            this._split.Panel1.ResumeLayout(false);
            this._split.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._split)).EndInit();
            this._split.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView lvMain;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.Button _remove;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel _port;
        private System.Windows.Forms.ToolStripStatusLabel _status;
        private System.Windows.Forms.NotifyIcon _notify;
        private System.Windows.Forms.ContextMenuStrip _notify_strip;
        private System.Windows.Forms.ToolStripMenuItem __active;
        private System.Windows.Forms.ToolStripMenuItem __startup;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem fileTransferLimitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __quota0;
        private System.Windows.Forms.ToolStripMenuItem __quota1;
        private System.Windows.Forms.ToolStripMenuItem __quota2;
        private System.Windows.Forms.ToolStripMenuItem __quota3;
        private System.Windows.Forms.ToolStripMenuItem notificationToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem __notify0;
        private System.Windows.Forms.ToolStripMenuItem __notify1;
        private System.Windows.Forms.ToolStripMenuItem __notify2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem __about;
        private System.Windows.Forms.ToolStripMenuItem __quit;
        private System.Windows.Forms.ToolStripMenuItem __restart;
        private System.Windows.Forms.SplitContainer _split;
        private System.Windows.Forms.ListBox _log;
        private System.Windows.Forms.ToolStripMenuItem __invite;
    }
}

