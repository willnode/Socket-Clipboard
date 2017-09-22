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
            this._add = new System.Windows.Forms.Button();
            this._cancel = new System.Windows.Forms.Button();
            this._list = new System.Windows.Forms.ListView();
            this._manual = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // _add
            // 
            this._add.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._add.Location = new System.Drawing.Point(135, 182);
            this._add.Name = "_add";
            this._add.Size = new System.Drawing.Size(75, 23);
            this._add.TabIndex = 0;
            this._add.Text = "Add";
            this._add.UseVisualStyleBackColor = true;
            this._add.Click += new System.EventHandler(this._add_Click);
            // 
            // _cancel
            // 
            this._cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this._cancel.Location = new System.Drawing.Point(216, 182);
            this._cancel.Name = "_cancel";
            this._cancel.Size = new System.Drawing.Size(75, 23);
            this._cancel.TabIndex = 1;
            this._cancel.Text = "Cancel";
            this._cancel.UseVisualStyleBackColor = true;
            this._cancel.Click += new System.EventHandler(this._cancel_Click);
            // 
            // _list
            // 
            this._list.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this._list.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._list.CheckBoxes = true;
            this._list.FullRowSelect = true;
            this._list.Location = new System.Drawing.Point(-1, 46);
            this._list.Name = "_list";
            this._list.Size = new System.Drawing.Size(304, 124);
            this._list.TabIndex = 2;
            this._list.UseCompatibleStateImageBehavior = false;
            this._list.View = System.Windows.Forms.View.List;
            // 
            // _manual
            // 
            this._manual.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._manual.Location = new System.Drawing.Point(9, 182);
            this._manual.Name = "_manual";
            this._manual.Size = new System.Drawing.Size(75, 23);
            this._manual.TabIndex = 3;
            this._manual.Text = "Manual";
            this._manual.UseVisualStyleBackColor = true;
            this._manual.Click += new System.EventHandler(this._manual_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(303, 43);
            this.label1.TabIndex = 4;
            this.label1.Text = "These are hostnames that detected on your LAN. \r\nSelect new hosts that you permit" +
    " to read your clipboard data.";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // Inviter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(303, 213);
            this.Controls.Add(this.label1);
            this.Controls.Add(this._manual);
            this.Controls.Add(this._list);
            this.Controls.Add(this._cancel);
            this.Controls.Add(this._add);
            this.Name = "Inviter";
            this.Text = "Add new Clients";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button _add;
        private System.Windows.Forms.Button _cancel;
        private System.Windows.Forms.ListView _list;
        private System.Windows.Forms.Button _manual;
        private System.Windows.Forms.Label label1;
    }
}