using System;
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
                case DataType.Text:
                    Clipboard.SetText((string)Data);
                    break;
                case DataType.HTML:
                    Clipboard.SetText((string)Data, TextDataFormat.Html);
                    break;
                case DataType.CSV:
                    Clipboard.SetText((string)Data, TextDataFormat.CommaSeparatedValue);
                    break;
                case DataType.RTF:
                    Clipboard.SetText((string)Data, TextDataFormat.Rtf);
                    break;
                case DataType.Image:
                    Clipboard.SetImage((Image)Data);
                    break;
                case DataType.File:
                    var buffer = Data as FileBuffer;
                    var tmp = Utility.SendToTemporary(buffer);
                    var list = new StringCollection();
                    list.Add(tmp);
                    Clipboard.SetFileDropList(list);
                    break;
                case DataType.Files:
                    var buffers = Data as FileBuffer[];
                    var lists = Utility.SendToTemporary(buffers);
                    Clipboard.SetFileDropList(lists);
                    break;
                default:
                    break;
            }
        }

        public static ClipData FromClipboard ()
        {
            if (Clipboard.ContainsText(TextDataFormat.Html))
                return new ClipData(DataType.HTML, Clipboard.GetText(TextDataFormat.Html));
            else if (Clipboard.ContainsText(TextDataFormat.Rtf))
                return new ClipData(DataType.RTF, Clipboard.GetText(TextDataFormat.Rtf));
            else if (Clipboard.ContainsText(TextDataFormat.CommaSeparatedValue))
                return new ClipData(DataType.CSV, Clipboard.GetText(TextDataFormat.CommaSeparatedValue));
            else if (Clipboard.ContainsText())
                return new ClipData(DataType.Text, Clipboard.GetText());
            else if (Clipboard.ContainsImage())
                return new ClipData(DataType.Image,Clipboard.GetImage());
            else if (Clipboard.ContainsFileDropList())
               return Utility.FileDropsToBuffer(Clipboard.GetFileDropList());
            else
                return null;
        }

        public long GetSize ()
        {
            switch (Type)
            {
                case DataType.Text:
                case DataType.CSV:
                case DataType.HTML:
                case DataType.RTF:
                    return ((string)Data).Length;
                case DataType.Image:
                    var sz = ((Image)Data).Size;
                    return sz.Height * sz.Width;
                case DataType.File:
                    return ((FileBuffer)Data).content.Length;
                default:
                    return -1;
            }
        }

        public string GetSizeReadable ()
        {
            switch(Type)
            {
                case DataType.Text:
                case DataType.CSV:
                case DataType.HTML:
                case DataType.RTF:
                    return ((string)Data).Length.ToString() + " chr";
                case DataType.Image:
                    var sz = ((Image)Data).Size;
                    return string.Format("{0} x {1} px", sz.Width, sz.Height);
                case DataType.File:
                    return Utility.GetBytesReadable(((FileBuffer)Data).content.Length);
                case DataType.Files:
                    return ((FileBuffer[])Data).Length + " files";
                default:
                    return "";
            }
        }

    }

    [Serializable]
    public class FileBuffer
    {
        public byte[] content;
        public string name;
        public DateTime modified;
        
        public FileBuffer ()
        {

        }

        public FileBuffer (FileInfo file)
        {
            content = File.ReadAllBytes(file.FullName);
            name = file.Name;
            modified = file.LastWriteTime;
        }
    }

    public enum DataType
    {
        Text = 0,
        RTF = 1,
        HTML = 2,
        CSV = 3,
        Image = 4,
        File = 5,
        Files = 6,
    }
}
