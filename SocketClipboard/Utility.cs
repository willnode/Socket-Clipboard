using System;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;

namespace SocketClipboard
{
    public static class Utility
    {
        public static DialogResult ShowInputDialog(ref string input, string title = "Input Box")
        {
            Size size = new Size(200, 70);
            Form inputBox = new Form();

            inputBox.FormBorderStyle = FormBorderStyle.FixedDialog;
            inputBox.MinimizeBox = false;
            inputBox.MaximizeBox = false;
            inputBox.ClientSize = size;
            inputBox.StartPosition = FormStartPosition.CenterParent;
            inputBox.Text = title;

            TextBox textBox = new TextBox();
            textBox.Size = new Size(size.Width - 10, 23);
            textBox.Location = new Point(5, 5);
            textBox.Text = input;
            inputBox.Controls.Add(textBox);

            Button okButton = new Button();
            okButton.DialogResult = DialogResult.OK;
            okButton.Name = "okButton";
            okButton.Size = new Size(75, 23);
            okButton.Text = "&OK";
            okButton.Location = new Point(size.Width - 80 - 80, 39);
            inputBox.Controls.Add(okButton);

            Button cancelButton = new Button();
            cancelButton.DialogResult = DialogResult.Cancel;
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(75, 23);
            cancelButton.Text = "&Cancel";
            cancelButton.Location = new Point(size.Width - 80, 39);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        //const int maxSize = 1024 * 1024 * 50; // 50 MB Max file size
        static readonly long[] maxSizes = new long[] { 1024 * 1024, 1024 * 1024 * 50, 1024 * 1024 * 1024, long.MaxValue };

        public static ClipData FileDropsToBuffer (StringCollection paths)
        {
            try
            {
                var maxSize = maxSizes[Main.limit];
                {
                    var files = new FileBuffer();
                    var len = 0L;
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (File.Exists(paths[i]))
                        {
                            var file = new FileInfo(paths[i]);
                            if ((len += file.Length) > maxSize)
                            {
                                Main.current.Log(NotificationType.BufferDiscarded, GetBytesReadable(len));
                                return null;
                            }
                            files.Add(file);
                        }
                    }
                    return new ClipData(DataType.Files, files);
                }
            }
            catch (Exception)
            {
                return null;
            }
            
        }

        public static string SendToTemporary (string name, byte[] content, DateTime modified)
        {
            var temp = Path.GetTempPath();
            temp += "\\" + name;
            File.WriteAllBytes(temp, content);
            File.SetLastWriteTime(temp, modified);

            return temp;
        }

        public static StringCollection SendToTemporary(FileBuffer buffer)
        {
            var s = new StringCollection();
            for (int i = 0; i < buffer.name.Count; i++)
            {
               s.Add(SendToTemporary(buffer.name[i], buffer.content[i], buffer.modified[i]));
            }
            return s;
        }

        public static IPAddress GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip;
                }
            }
            return IPAddress.Any;
        }

        // Returns the human-readable file size for an arbitrary, 64-bit file size 
        // The default format is "0.### XB", e.g. "4.2 KB" or "1.434 GB"
        public static string GetBytesReadable(long i)
        {
            // Get absolute value
            long absolute_i = (i < 0 ? -i : i);
            // Determine the suffix and readable value
            string suffix;
            double readable;
            if (absolute_i >= 0x1000000000000000) // Exabyte
            {
                suffix = "EB";
                readable = (i >> 50);
            }
            else if (absolute_i >= 0x4000000000000) // Petabyte
            {
                suffix = "PB";
                readable = (i >> 40);
            }
            else if (absolute_i >= 0x10000000000) // Terabyte
            {
                suffix = "TB";
                readable = (i >> 30);
            }
            else if (absolute_i >= 0x40000000) // Gigabyte
            {
                suffix = "GB";
                readable = (i >> 20);
            }
            else if (absolute_i >= 0x100000) // Megabyte
            {
                suffix = "MB";
                readable = (i >> 10);
            }
            else if (absolute_i >= 0x400) // Kilobyte
            {
                suffix = "KB";
                readable = i;
            }
            else
            {
                return i.ToString("0 B"); // Byte
            }
            // Divide by 1024 to get fractional value
            readable = (readable / 1024.0);
            // Return formatted number with suffix
            return readable.ToString("0.00 ") + suffix;
        }
    }

    public class NetClient : TcpClient
    {
        public string Name;

        public NetClient (string name)
        {
            Name = name;
            
        }
    }
}
