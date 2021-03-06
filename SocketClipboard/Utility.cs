﻿using System;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;

namespace SocketClipboard
{
    public static class Utility
    {
        public static DialogResult ShowInputDialog(ref string input, string title = "Input Box")
        {
            Size size = new Size(200, 70);

            Form inputBox = new Form()
            {
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MinimizeBox = false,
                MaximizeBox = false,
                ClientSize = size,
                StartPosition = FormStartPosition.CenterParent,
                Text = title,
            };

            TextBox textBox = new TextBox()
            {
                Size = new Size(size.Width - 10, 23),
                Location = new Point(5, 5),
                Text = input,
            };


            Button okButton = new Button()
            {
                DialogResult = DialogResult.OK,
                Name = "okButton",
                Size = new Size(75, 23),
                Text = "&OK",
                Location = new Point(size.Width - 80 - 80, 39),
            };


            Button cancelButton = new Button()
            {
                DialogResult = DialogResult.Cancel,
                Name = "cancelButton",
                Size = new Size(75, 23),
                Text = "&Cancel",
                Location = new Point(size.Width - 80, 39),
            };

            inputBox.Controls.Add(textBox);
            inputBox.Controls.Add(okButton);
            inputBox.Controls.Add(cancelButton);

            inputBox.AcceptButton = okButton;
            inputBox.CancelButton = cancelButton;

            DialogResult result = inputBox.ShowDialog();
            input = textBox.Text;
            return result;
        }

        public static FileBuffer FileDropsToBuffer(string[] paths)
        {
            var policy = Main.Current.config.FileTransfer;
            if (policy == Main.FileTransferFlag.Block | (paths.Length > 1 & policy == Main.FileTransferFlag.Single)) return null;

            try
            {
                var files = new FileBuffer();

                for (int i = 0; i < paths.Length; i++)
                {
                    if (File.Exists(paths[i]))
                    {
                        files.Add(new FileInfo(paths[i]));
                    }
                }

                for (int i = 0; i < paths.Length; i++)
                {
                    if (Directory.Exists(paths[i]))
                    {
                        var root = Path.GetDirectoryName(paths[i]);

                        foreach (var filePath in Directory.EnumerateFiles(paths[i], "*", SearchOption.AllDirectories))
                        {
                            files.Add(new FileInfo(filePath), root);
                        }
                    }
                }

                return files;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static readonly string DumpDestination = Path.GetTempPath() + "\\SocketDumps\\";

        public const int MultiPacketCap = 1024 * 256;

        public static void SetupTemporaryFiles(FileBuffer buffer)
        {
            // Cleanup dirs
            Directory.CreateDirectory(DumpDestination);
            var dir = new DirectoryInfo(DumpDestination);
            foreach (var file in dir.EnumerateFiles()) file.Delete();
            foreach (var fold in dir.EnumerateDirectories()) fold.Delete(true);

            // Make empty directories
            foreach (var file in buffer.files)
                Directory.CreateDirectory(Path.GetDirectoryName(file.destination));
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
            return null;
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

        public static bool HaveAdminAccess
        {
            get
            {
                WindowsIdentity identity = WindowsIdentity.GetCurrent();
                WindowsPrincipal principal = new WindowsPrincipal(identity);
                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
            set
            {
                if (!value || HaveAdminAccess) return;
                // Restart
                var exe = Process.GetCurrentProcess().MainModule.FileName;
                var strt = new ProcessStartInfo(exe) { Verb = "runas" };
                Process.Start(strt);
                Environment.Exit(0);
            }
        }

        public static Version GetVersion()
        {
            return AssemblyName.GetAssemblyName(Assembly.GetExecutingAssembly().Location).Version;
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

        /// <summary> Flash the window <remarks>
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

        public static bool Contains(this List<NetClient> list, string name)
        {
            return IndexOf(list, name) >= 0;
        }

        public static int IndexOf(this List<NetClient> list, string name)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Name == name) return i;
            }
            return -1;
        }

        public static string DeepMessage(this Exception ex)
        {
            return (ex.InnerException == null ? ex.Message : ex.InnerException.Message);
        }
    }

    public class NetClient : TcpClient
    {
        public string Name;

        public string Log = "...";

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

    /// <summary>
    /// StreamQueue provides an alternative implementation to High speed 
    /// IO/Stream operations that works on multitheaded environment.
    /// Internally use 'circular array' in the implementation.
    /// </summary>
    public class StreamQueue
    {
        byte[] _array = new byte[64]; // array save
        int _head;       // head index
        int _tail;       // tail index
        int _size;       // Number of elements.

        /// <summary>
        /// Buffer size for stream operation
        /// </summary>
        const int BufferBlock = 1024 * 256;

        /// <summary>
        /// Get total bytes in queue
        /// </summary>
        public int Count
        {
            get { return _size; }
        }

        /// <summary>
        /// Remove all bytes in queue
        /// </summary>
        public virtual void Clear()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
        }

        /// <summary>
        /// Remove all bytes in queue and discard waste memory to GC.
        /// </summary>
        public void Clean ()
        {
            _head = 0;
            _tail = 0;
            _size = 0;
            _array = new byte[64];
        }

        /// <summary>
        /// Push a byte to queue.
        /// </summary>
        public virtual void Enqueue(byte obj)
        {
            if (_size == _array.Length)
            {
                SetCapacity(_size << 1);
            }

            _array[_tail] = obj;
            _tail = (_tail + 1) % _array.Length;
            _size++;
        }
        
        /// <summary>
        /// Pop a byte from queue.
        /// </summary>
        /// <returns></returns>
        public byte Dequeue()
        {
            if (_size == 0)
                throw new InvalidOperationException("Invalid operation");

            byte removed = _array[_head];
            _head = (_head + 1) % _array.Length;
            _size--;
            return removed;
        }

        /// <summary>
        /// Read and push stream bytes to queue. Can be multi-threaded.
        /// </summary>
        /// <param name="stream">The stream to push</param>
        /// <param name="maxSize">Maximum bytes size for this operation</param>
        /// <returns>Size of pushed bytes</returns>
        public int Enqueue(Stream stream, long maxSize = int.MaxValue)
        {
            if ((_size + BufferBlock) > _array.Length)
            {
                SetCapacity(Math.Max((_size + BufferBlock), _size << 1));
            }

            var count = Math.Min(Math.Min((int)maxSize, BufferBlock), _array.Length - _tail);

            lock (_array)
            {
                count = stream.Read(_array, _tail, count);
                _tail = (_tail + count) % _array.Length;
                _size += count;
            }

            return count;
        }

        /// <summary>
        /// Pop and write stream bytes to queue. Can be multi-threaded.
        /// </summary>
        /// <param name="stream">The stream to pop</param>
        /// <param name="maxSize">Maximum bytes size for this operation</param>
        /// <returns>Size of popped bytes</returns>
        public int Dequeue(Stream stream, long maxSize = int.MaxValue)
        {
            if (_size == 0)
                return 0; // It's okay to be empty

            int count = Math.Min(Math.Min(BufferBlock, _size), Math.Min((int)maxSize, _array.Length - _head));

            lock (_array)
            {
                stream.Write(_array, _head, count);
                _head = (_head + count) % _array.Length;
                _size -= count;
            }

            return count;
        }

        /// <summary>
        /// Set new capacity
        /// </summary>
        private void SetCapacity(int capacity)
        {
            lock (_array)
            {
                byte[] newarray = new byte[capacity];
                if (_size > 0)
                {
                    if (_head < _tail)
                    {
                        Array.Copy(_array, _head, newarray, 0, _size);
                    }
                    else
                    {
                        Array.Copy(_array, _head, newarray, 0, _array.Length - _head);
                        Array.Copy(_array, 0, newarray, _array.Length - _head, _tail);
                    }
                }

                _array = newarray;
                _head = 0;
                _tail = (_size == capacity) ? 0 : _size;
            }
        }


    }
}
