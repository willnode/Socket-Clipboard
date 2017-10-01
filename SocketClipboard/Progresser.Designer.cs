namespace SocketClipboard
{
    partial class Progresser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Progresser));
            this._prog = new System.Windows.Forms.ProgressBar();
            this._l = new System.Windows.Forms.Label();
            this._r = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _prog
            // 
            this._prog.Dock = System.Windows.Forms.DockStyle.Fill;
            this._prog.ForeColor = System.Drawing.SystemColors.ControlText;
            this._prog.Location = new System.Drawing.Point(80, 0);
            this._prog.MarqueeAnimationSpeed = 1;
            this._prog.Name = "_prog";
            this._prog.Size = new System.Drawing.Size(213, 42);
            this._prog.TabIndex = 0;
            this._prog.Value = 50;
            // 
            // _l
            // 
            this._l.Dock = System.Windows.Forms.DockStyle.Left;
            this._l.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._l.Location = new System.Drawing.Point(0, 0);
            this._l.Margin = new System.Windows.Forms.Padding(3);
            this._l.MinimumSize = new System.Drawing.Size(0, 20);
            this._l.Name = "_l";
            this._l.Size = new System.Drawing.Size(80, 42);
            this._l.TabIndex = 1;
            this._l.Text = "2.00 MB\r\n4.00 MB";
            this._l.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _r
            // 
            this._r.Dock = System.Windows.Forms.DockStyle.Right;
            this._r.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this._r.Location = new System.Drawing.Point(293, 0);
            this._r.Margin = new System.Windows.Forms.Padding(3);
            this._r.MinimumSize = new System.Drawing.Size(0, 20);
            this._r.Name = "_r";
            this._r.Size = new System.Drawing.Size(81, 42);
            this._r.TabIndex = 2;
            this._r.Text = "( 0 / 0 )\r\nFile.apx";
            this._r.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Progresser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 42);
            this.Controls.Add(this._prog);
            this.Controls.Add(this._l);
            this.Controls.Add(this._r);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Progresser";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Files in progress ...";
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Progresser_FormClosing);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar _prog;
        private System.Windows.Forms.Label _l;
        private System.Windows.Forms.Label _r;
    }
}