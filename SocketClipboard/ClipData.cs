using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SocketClipboard
{

    [Serializable]
    public abstract class ClipBuffer
    {
        public abstract DataType Type { get; }

        public abstract void ToClipboard();

        public abstract long GetSize();

        public virtual string GetSizeReadable() { return Utility.GetBytesReadable(GetSize()); }

        public static ClipBuffer FromClipboard()
        {
            var obj = Clipboard.GetDataObject();
            if (obj == null)
                return null;
            else if (obj.GetDataPresent(DataFormats.FileDrop))
                return Utility.FileDropsToBuffer((string[])obj.GetData(DataFormats.FileDrop, true));
            else if (obj.GetDataPresent(typeof(string)))
                return TextBuffer.FromClipboard(obj);
            else if (obj.GetDataPresent(typeof(Bitmap)))
                return BitmapBuffer.FromClipboard(obj);
            else
                return null;
        }
    }

    [Serializable]
    public struct FileBufferUnit
    {
        /// Size of the file
        public long size;
        /// Relative name + directory path
        public string name;
        /// True if this file will transmitted broadcastly
        public bool multiStaged;
        /// File datestamp
        public DateTime modified;
        /// Source path (useful only for sender)
        [NonSerialized]
        public string source;

        public string destination { get { return Utility.DumpDestination + name; } }
    }

    [Serializable]
    public class FileBuffer : ClipBuffer
    {

        public List<FileBufferUnit> files = new List<FileBufferUnit>();

        public long totalSize;

        public override DataType Type { get { return DataType.Files; } }

        public FileBuffer() { }

        public void Add(FileInfo file)
        {
            AddInternal(file, file.Name);
        }

        public void Add(FileInfo file, string root)
        {
            AddInternal(file, file.FullName.Replace(root, "").Substring(1));
        }

        void AddInternal (FileInfo file, string name)
        {
            files.Add(new FileBufferUnit()
            {
                name = name,
                size = file.Length,
                source = file.FullName,
                modified = file.LastWriteTime,
                multiStaged = file.Length > Utility.SinglePacketCap,
            });

            totalSize += file.Length;
        }

        public override string ToString()
        {
            return files.Count >= 1 ? files.Count.ToString() + " files" : "a file";
        }

        public override void ToClipboard()
        {
            var lists = new StringCollection();

            foreach (var file in files)
            {
                var idx = file.name.IndexOf('\\');
                if (idx >= 0)
                {
                    var dst = Utility.DumpDestination + file.name.Substring(0, idx);
                    if (!lists.Contains(dst)) lists.Add(dst);  // only copy root dir
                }                   
                else
                    lists.Add(file.destination);
            }

            var data = new DataObject();
            data.SetFileDropList(lists);
            data.SetData("Preferred DropEffect", DragDropEffects.Move); // Cut
            Clipboard.SetDataObject(data);
        }

        public override long GetSize()
        {
            return totalSize;
        }

        public bool RequireAsyncStatus()
        {
            return totalSize > Utility.MultiPacketCap;
        }
    }

    [Serializable]
    public abstract class DirectBuffer<T> : ClipBuffer
    {
        public List<string> type = new List<string>();

        public List<T> data = new List<T>();

        public void Add(string type, T data)
        {
            this.type.Add(type);
            this.data.Add(data);
        }

        public override void ToClipboard()
        {
            var r = new DataObject();
            for (int i = 0; i < type.Count; i++)
            {
                if (data[i] != null)
                    r.SetData(type[i], data[i]);
            }
            Clipboard.SetDataObject(r);
        }


    }

    [Serializable]
    public class TextBuffer : DirectBuffer<string>
    {

        // Known convertible string formats
        static readonly string[] dataTypes = new string[] { DataFormats.Html, DataFormats.Rtf, DataFormats.CommaSeparatedValue, DataFormats.Text, DataFormats.UnicodeText };

        public override DataType Type { get { return DataType.Strings; } }

        public TextBuffer() { }

        public static TextBuffer FromClipboard(IDataObject data)
        {
            var r = new TextBuffer();

            foreach (var type in dataTypes)
            {
                if (data.GetDataPresent(type))
                    r.Add(type, data.GetData(type, true) as string);
            }

            return r;
        }

        public override string ToString()
        {
            return "a string data";
        }

        public override long GetSize()
        {
            long size = 0;
            foreach (var d in data)
                size += d.Length;

            return size;
        }
    }

    [Serializable]
    public class BitmapBuffer : DirectBuffer<Bitmap>
    {

        // Known convertible string formats
        static readonly string[] dataTypes = new string[] { DataFormats.Bitmap, DataFormats.Dib };

        public override DataType Type { get { return DataType.Image; } }

        public BitmapBuffer() { }

        public static BitmapBuffer FromClipboard(IDataObject data)
        {
            var r = new BitmapBuffer();

            foreach (var type in dataTypes)
            {
                if (data.GetDataPresent(type))
                    r.Add(type, data.GetData(type, true) as Bitmap);
            }

            return r;
        }

        public override string ToString()
        {
            return "a bitmap data";
        }

        public override string GetSizeReadable()
        {
            if (data.Count == 1)
                return data[0].Width + " x " + data[0].Height;
            else
                return base.GetSizeReadable();
        }

        public override long GetSize()
        {
            long size = 0;
            foreach (var d in data)
                if (d != null) size += d.Height * d.Width * 4;

            return size;
        }
    }

    public enum DataType
    {
        Files = 0,
        Strings = 1,
        Image = 2,
    }
}
