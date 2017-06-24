using System;
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

        const int maxSize = 1024 * 1024 * 50; // 50 MB Max file size

        public static ClipData FileDropsToBuffer (StringCollection paths)
        {
            try
            {
                if (paths.Count == 1)
                {
                    if (!File.Exists(paths[0]))
                        return null;
                    var file = new FileInfo(paths[0]);
                    if (file.Length > maxSize)
                    {
                        Console.Write("File not copied because it's larger than 50 MB");
                        return null;
                    }
                    else
                        return new ClipData(DataType.File, new FileBuffer(file));
                } else
                {
                    var files = new FileBuffer[paths.Count];
                    var len = 0L;
                    for (int i = 0; i < paths.Count; i++)
                    {
                        if (File.Exists(paths[i]))
                        {
                            var file = new FileInfo(paths[i]);
                            len += file.Length;
                            if (len > maxSize)
                                break;
                            files[i] = new FileBuffer(file);
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

        public static string SendToTemporary (FileBuffer buffer)
        {
            var temp = Path.GetTempPath();
            temp += "\\" + buffer.name;
            File.WriteAllBytes(temp, buffer.content);
            File.SetLastWriteTime(temp, buffer.modified);

            return temp;
        }

        public static StringCollection SendToTemporary(FileBuffer[] buffer)
        {
            var s = new StringCollection();
            for (int i = 0; i < buffer.Length; i++)
            {
                if (buffer[i] != null)
                    s.Add(SendToTemporary(buffer[i]));
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
            readable = (readable / 1024);
            // Return formatted number with suffix
            return readable.ToString("0.## ") + suffix;
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
