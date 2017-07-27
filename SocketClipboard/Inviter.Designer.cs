namespace SocketClipboard
{
    partial class Inviter
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
            this._quit = new System.Windows.Forms.Button();
            this._st1 = new System.Windows.Forms.Label();
            this._st2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _quit
            // 
            this._quit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._quit.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this._quit.Location = new System.Drawing.Point(254, 44);
            this._quit.Name = "_quit";
            this._quit.Size = new System.Drawing.Size(75, 23);
            this._quit.TabIndex = 0;
            this._quit.Text = "Abort";
            this._quit.UseVisualStyleBackColor = true;
            // 
            // _st1
            // 
            this._st1.AutoSize = true;
            this._st1.Location = new System.Drawing.Point(12, 19);
            this._st1.Name = "_st1";
            this._st1.Size = new System.Drawing.Size(67, 13);
            this._st1.TabIndex = 1;
            this._st1.Text = "Please wait..";
            // 
            // _st2
            // 
            this._st2.AutoSize = true;
            this._st2.Location = new System.Drawing.Point(12, 49);
            this._st2.Name = "_st2";
            this._st2.Size = new System.Drawing.Size(167, 13);
            this._st2.TabIndex = 2;
            this._st2.Text = "Reaching 0 clients, 0 confirmation";
            // 
            // Inviter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this._quit;
            this.ClientSize = new System.Drawing.Size(341, 78);
            this.ControlBox = false;
            this.Controls.Add(this._st2);
            this.Controls.Add(this._st1);
            this.Controls.Add(this._quit);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Inviter";
            this.ShowInTaskbar = false;
            this.Text = "Sending Invitation...";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Inviter_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button _quit;
        private System.Windows.Forms.Label _st1;
        private System.Windows.Forms.Label _st2;
    }
}