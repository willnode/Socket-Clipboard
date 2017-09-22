using System;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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

        static readonly long[] maxSizes = new long[] { 1024 * 1024, 1024 * 1024 * 50, 1024 * 1024 * 1024, long.MaxValue };

        public static FileBuffer FileDropsToBuffer(string[] paths)
        {
            try
            {
                var maxSize = maxSizes[Main.limit];
                {
                    var files = new FileBuffer();
                    var len = 0L;
                    for (int i = 0; i < paths.Length; i++)
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
                    for (int i = 0; i < paths.Length; i++)
                    {
                        if (Directory.Exists(paths[i]))
                        {
                            var iter = Directory.EnumerateFiles(paths[i], "*", SearchOption.AllDirectories);
                            foreach (var filePath in iter)
                            {
                                var file = new FileInfo(filePath);
                                if ((len += file.Length) > maxSize)
                                {
                                    Main.current.Log(NotificationType.BufferDiscarded, GetBytesReadable(len));
                                    return null;
                                }
                                files.Add(file, Path.GetDirectoryName(paths[i]));
                            }
                        }
                    }
                    return files;
                }
            }
            catch (Exception)
            {
                return null;
            }

        }

        public static readonly string DumpDestination = Path.GetTempPath() + "\\SocketDumps\\";

        public const long SinglePacketCap = 1024 * 1024 * 16; // 16 MB is max file size for single-time transmit

        public const long MultiPacketCap = 1024 * 1024 * 1; // Large files only

        public static void SetupTemporaryFiles(FileBuffer buffer)
        {
            // Cleanup dirs
            Directory.CreateDirectory(DumpDestination);
            var dir = new DirectoryInfo(DumpDestination);
            foreach (var file in dir.EnumerateFiles()) file.Delete();
            foreach (var fold in dir.EnumerateDirectories()) fold.Delete(true);

            foreach (var file in buffer.files)
            {
                Directory.CreateDirectory(Path.GetDirectoryName(file.destination));
            }
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

        public static string Truncate(this string s, int length)
        {
            if (s.Length <= length) return s;
            else return s.Substring(0, length);
        }


        // To support flashing.
        [DllImport("user32.dll", CallingConvention = CallingConvention.Cdecl)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool FlashWindowEx(ref FLASHWINFO pwfi);

        //Flash both the window caption and taskbar button.
        //This is equivalent to setting the FLASHW_CAPTION | FLASHW_TRAY flags. 
        private const uint FLASHW_ALL = 3;

        // Flash continuously until the window comes to the foreground. 
        private const uint FLASHW_TIMERNOFG = 12;

        [StructLayout(LayoutKind.Sequential)]
        private struct FLASHWINFO
        {
            public uint cbSize;
            public IntPtr hwnd;
            public uint dwFlags;
            public uint uCount;
            public uint dwTimeout;
        }

        /// <summary>
        /// Send form taskbar notification, the Window will flash until get's focus
        /// <remarks>
        /// This method allows to Flash a Window, signifying to the user that some major event occurred within the application that requires their attention. 
        /// </remarks>
        /// </summary>
        /// <param name="form"></param>
        /// <returns></returns>
        public static bool Flash(this Form form)
        {
            IntPtr hWnd = form.Handle;
            FLASHWINFO fInfo = new FLASHWINFO();

            fInfo.cbSize = Convert.ToUInt32(Marshal.SizeOf(fInfo));
            fInfo.hwnd = hWnd;
            fInfo.dwFlags = FLASHW_ALL | FLASHW_TIMERNOFG;
            fInfo.uCount = 2;
            fInfo.dwTimeout = 500;

            return FlashWindowEx(ref fInfo);
        }
    }

    public class NetClient : TcpClient
    {
        public string Name;

        public NetClient(string name) : base()
        {
            Name = name;
        }
    }

    public class NetListener : TcpListener
    {

        public NetListener(IPEndPoint local) : base(local) { }

        public NetListener(IPAddress ip, int port) : base(ip, port) { }

        public new bool Active
        {
            get
            {
                return base.Active;
            }
        }
    }
}
