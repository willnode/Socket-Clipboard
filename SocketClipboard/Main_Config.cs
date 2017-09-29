using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Win32;

namespace SocketClipboard
{

    public partial class Main
    {

        public class SocketConfig
        {

            public Main main;
            public FileTransferFlag FileTransfer = FileTransferFlag.Allow;
            public NotifyFlag Notify = NotifyFlag.Informative;
            public bool Active = true;
            public bool Mute;
            public bool Solo;
            public int Port = 5000;

            public bool RunAtStartup
            {
                get
                {
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey
                       ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
                    return (string)rk.GetValue("Socket-Clipboard", null) == Application.ExecutablePath + " --background";
                }
                set
                {
                    RegistryKey rk = Registry.CurrentUser.OpenSubKey
                        ("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

                    if (value)
                        rk.SetValue("Socket-Clipboard", Application.ExecutablePath + " --background");
                    else
                        rk.DeleteValue("Socket-Clipboard", false);
                }
            }

            public bool Offline
            {
                get { return main.server == null; }
            }


            static RegistryKey GetRegPath()
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey("Software", true);

                key.CreateSubKey("SocketClipboard");
                return key.OpenSubKey("SocketClipboard", true);
            }

            public void Init(Main main)
            {
                this.main = main;
                var reg = GetRegPath();
                Active = (int)reg.GetValue("ACTIVE", 1) > 0;
                Mute = (int)reg.GetValue("MUTE", 0) > 0;
                Solo = (int)reg.GetValue("SOLO", 0) > 0;
                Port = (int)reg.GetValue("PORT", Port);
                FileTransfer = (FileTransferFlag)reg.GetValue("FILE", FileTransfer);
                Notify = (NotifyFlag)reg.GetValue("NOTIFY", Notify);

                var hosts2 = reg.GetValue("HOSTS", null) as string;
                if (hosts2 != null)
                {
                    var hosts = hosts2.Split('|');
                    foreach (var host in hosts) main.AddHost(host);
                }

                main.__file0.Tag = FileTransferFlag.Block;
                main.__file1.Tag = FileTransferFlag.Single;
                main.__file2.Tag = FileTransferFlag.Allow;
                main.__notify0.Tag = NotifyFlag.Silent;
                main.__notify1.Tag = NotifyFlag.Informative;
                main.__notify2.Tag = NotifyFlag.Verbose;

                Validate();
            }

            public void Validate()
            {
                main.___active.Checked = main.__active.Checked = Active;
                main.__mute.Checked = Mute;
                main.__solo.Checked = Solo;
                main.__file0.Checked = FileTransfer == FileTransferFlag.Block;
                main.__file1.Checked = FileTransfer == FileTransferFlag.Single;
                main.__file2.Checked = FileTransfer == FileTransferFlag.Allow;
                main.__notify0.Checked = Notify == NotifyFlag.Silent;
                main.__notify1.Checked = Notify == NotifyFlag.Informative;
                main.__notify2.Checked = Notify == NotifyFlag.Verbose;
                main._port.Text = "PORT: " + Port.ToString();
                if (!Offline)
                {
                    main.Text = "Socket-Clipboard " +
                   (Active ? string.Empty : "(Passive) ") +
                   (Mute ? "(Muted) " : string.Empty) +
                   (Solo ? "(Soloed to " + (main.soloIndex == -1 ? "<null>" : main.clients[main.soloIndex].Name) + ")" : string.Empty);
                }
                else
                    main.Text = "Socket-Clipboard (Offline)";

            }

            public void Validate(bool save)
            {
                Validate();
                if (save) SaveConfig();
            }

            public void SaveConfig()
            {
                var reg = GetRegPath();
                reg.SetValue("ACTIVE", Active ? 1 : 0);
                reg.SetValue("SOLO", Solo ? 1 : 0);
                reg.SetValue("MUTE", Mute ? 1 : 0);
                reg.SetValue("PORT", Port);
                reg.SetValue("FILE", (int)FileTransfer);
                reg.SetValue("NOTIFY", (int)Notify);
            }

            public void SaveHosts ()
            {
                GetRegPath().SetValue("HOSTS", string.Join("|", main.clients.ConvertAll(x => x.Name)));
            }

            

        }

        public enum FileTransferFlag
        {
            Block = 0,
            Single = 1,
            Allow = 2,
        }

        public enum NotifyFlag
        {
            Silent = 0,
            Informative = 1,
            Verbose = 2,
            Internal = 3,
        }
    }
}
