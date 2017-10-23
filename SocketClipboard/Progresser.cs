using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClipboard
{
    public partial class Progresser : Form
    {

        public Progresser(Main main)
        {
            InitializeComponent();
            Visible = false;
            Opacity = 0;
            Show();
            this.main = main;
        }

        Main main;
        int files;
        long bytes;
        bool enabled;
        DateTime start;

        public void Init(FileBuffer buffer)
        {
            if (!(enabled = buffer.RequireAsyncStatus())) return;
            Invoke(new Action(() => Init(buffer.files.Count, buffer.totalSize, "")));
        }

        void Init(int totalFile, long totalByte, string target)
        {
            var wrk = Screen.PrimaryScreen.WorkingArea;
            this.SetDesktopLocation(wrk.Right - Width, wrk.Bottom - Height);

            files = totalFile;
            bytes = totalByte;
            Opacity = 1;
            Visible = true;
            start = DateTime.Now;
        }

        public void Done()
        {
            if (!enabled) return;
            Invoke(new Action(() =>
            {
                Text = "Finished! Paste files to your destination!";
                FadeOut();
            }));

        }

        async void FadeOut ()
        {
            this.Flash();
            await Task.Delay(2000);
            for (double i = 1; i >= 0; i-=0.01)
            {
                Opacity = i;
                await Task.Delay(10);
            }
            Visible = false;
            Opacity = 1;
        }

        public void Update(long curByte)
        {
            if (!enabled) return;
            Invoke(new Action(() =>
            {
                var time = (DateTime.Now - start);
                var speed = curByte / time.TotalSeconds;
                var phase = Math.Min(curByte / (double)bytes, 1.0);
                var remaining = TimeSpan.FromSeconds((1 - phase) * time.TotalSeconds / phase);
                _prog.Value = (int)(phase * 100);
                _l.Text = string.Format("{2}ps\r\n{0}\r\n{1}", Utility.GetBytesReadable(curByte),
                    Utility.GetBytesReadable(bytes), Utility.GetBytesReadable((long)speed));
                _r.Text = string.Format("{0:P1}\r\n {1:D2} m {2:D2} s\r\n {3:D2} m {4:D2} s", phase
                    , (int)time.TotalMinutes, time.Seconds, (int)remaining.TotalMinutes, remaining.Seconds);
            }));
        }

        public void Update(int curFile, string fileName)
        {
            if (!enabled) return;
            Invoke(new Action(() =>
            {
                Text = string.Format("Incoming file {0} of {1} : {2}", curFile, files, Path.GetFileName(fileName));
            }));
        }

        private void Progresser_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Not really exit, but sleeps
            if (e.CloseReason == CloseReason.UserClosing && main.ProgresserClosing())
            {
                e.Cancel = true;
                Visible = false;
            }
        }
    }
}
