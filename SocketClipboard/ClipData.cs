using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace SocketClipboard
{
    [Serializable]
    public class ClipData
    {
        public object Data;
        public DataType Type;

        public ClipData () { }

        public ClipData (DataType type, object data)
        {
            Data = data;
            Type = type;
        }

        public void SendToClipboard ()
        {
            switch (Type)
            {
                case DataType.Direct:
                    Clipboard.SetDataObject((Data as DirectBuffer).Apply());
                    break;
                case DataType.Files:
                    var buffers = Data as FileBuffer;
                    var lists = Utility.SendToTemporary(buffers);
                    var data = new DataObject();
                    data.SetFileDropList(lists);
                    data.SetData("Preferred DropEffect", DragDropEffects.Move); // Cut
                    Clipboard.SetDataObject(data);
                    break;
                default:
                    break;
            }
        }

        public static ClipData FromClipboard ()
        {
            try
            {
                if (Clipboard.ContainsFileDropList())
                    return Utility.FileDropsToBuffer(Clipboard.GetFileDropList());
                else
                    return new ClipData(DataType.Direct, DirectBuffer.FromClipboard(Clipboard.GetDataObject()));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetSizeReadable ()
        {
            switch (Type)
            {
                case DataType.Direct:
                    return Utility.GetBytesReadable((Data as DirectBuffer).GetSize());          
                case DataType.Files:
                    return Utility.GetBytesReadable((Data as FileBuffer).totalSize);
                default:
                    return "";
            }
        }

    }

    [Serializable]
    public class FileBuffer
    {
        public List<byte[]> content = new List<byte[]>();
        public List<string> name = new List<string>();
        public List<DateTime> modified = new List<DateTime>();
        public long totalSize;
        
        public FileBuffer ()
        {
        }

        public void Add (FileInfo file)
        {
            content.Add(File.ReadAllBytes(file.FullName));
            name.Add(file.Name);
            modified.Add(file.LastWriteTime);
            totalSize += file.Length;
        }

        public void Add (FileInfo file, string root)
        {       
            content.Add(File.ReadAllBytes(file.FullName));
            name.Add(file.FullName.Replace(root, "").Substring(1));
            modified.Add(file.LastWriteTime);
            totalSize += file.Length;
        }

        public override string ToString()
        {
            return name.Count >= 1 ? name.Count.ToString() + " files" : "a file";
        }
    }

    [Serializable]
    public class DirectBuffer
    {
        // Only serializable formats can get in
        static string[] dataTypes = new string[]
        {
            "Bitmap", "HTML Format", "Rich Text Format", "Csv", "UnicodeText", "Text",
            "DeviceIndepentBitmap"//, "TaggedImageFileFormat", "EnhancedMetafile", "MetaFilePict"
        };

        static string[] readableTypes = new string[]
        {
            "Bitmap", "HTML", "RTF", "CSV", "Text", "ASCII",
            "DIB"//, "TIFF", "Metafile", "Metafile",
        };

        public List<string> type = new List<string>();
        public List<object> data = new List<object>();

        public DirectBuffer ()
        {
        }

        public void Add (string type, object data)
        {
            this.type.Add(type);
            this.data.Add(data);
        }

        public static DirectBuffer FromClipboard (IDataObject data)
        {
            var r = new DirectBuffer();
            foreach (var type in dataTypes)
            {
              
                         r.Add(type, data.GetData(type, false));

            }
            return r.data.Count == 0 ? null : r;
        }

        public long GetSize ()
        {
            long size = 0;
            foreach (var d in data)
            {
                if (d is string)
                    size = Math.Max(size, (d as string).Length);
                else if (d is Image)
                {
                    var sz = (d as Image).Size;
                    size = Math.Max(size, sz.Width * sz.Height);
                }
            }
            return size;
        }

        public DataObject Apply ()
        {
            var r = new DataObject();
            for (int i = 0; i < type.Count; i++)
            {
                r.SetData(type[i], data[i]);
            }
            return r;
        }

        public override string ToString()
        {
            return "a " + readableTypes[Array.IndexOf(dataTypes, type[0])];
        }
    }

    [Serializable]
    public class InvitationBuffer
    {
        public string host;
        public string ip;
    }
    public enum DataType
    {
        Direct = 0,
        Files = 1,
        MetaInvitation = 2,
    }
}
