using System;
using System.Windows.Forms;
using Resources = SocketClipboard.Properties.Resources;
using System.Linq;

namespace SocketClipboard
{
    public enum NotificationType
    {
        Startup = 1,
        GoingOnline = 2,
        GoingOffline = 4,
        IPUpdated = 8,
        PortUpdated = 16,
        BufferDiscarded = 32,
        Sending = 64,
        Sent = 128,
        FailedSent = 256,
        Receiving = 512,
        Received = 1024,
        ClientAdded = 2048,
        Error = 0,
    }

    public partial class Main : Form
    {
       

        public void Log (NotificationType type, object data = null)
        {
            var file = data as ClipData;
            switch (type)
            {
                case NotificationType.Startup:
                    Log("Server online at " + server.LocalEndpoint.ToString());
                    Notify(verbose >= 1, "Socket-Clipboard is online", 
                        "Don't forget to run this software and list all your computer clients", StateLog.Normal);
                    break;
                case NotificationType.GoingOnline:
                    Log("Server is online at " + data.ToString() + ":" + port.ToString());
                    Notify(false, null, null, StateLog.Normal);
                    break;
                case NotificationType.GoingOffline:
                    Log("Server offline.");
                    Notify(false, null, null, StateLog.Offline);
                    break;
                case NotificationType.IPUpdated:
                    Log("IP updated at " + data.ToString());
                    break;
                case NotificationType.PortUpdated:
                    Log("Port updated at " + data.ToString());
                    break;
                case NotificationType.BufferDiscarded:
                    Log("Sending file(s) failed");
                    Notify(verbose >= 1, "Socket-Clipboard", "File(s) discarded because its total size (" + data.ToString() + "+) are excessing data limit. Please uplift the limit first.", StateLog.Normal, ToolTipIcon.Warning);
                    break;
                case NotificationType.Sending:
                    if (data == null)
                    {
                        Log("Emitting...");
                        Notify(false, null, null, StateLog.Busy);
                    }
                    else
                        Log(string.Format("Emitting ({0:P})", (float)data), true, true);
                    break;
                case NotificationType.Sent:
                    var reach = logs.Values.Count(x => "Sent" == x);
                    string str;
                    if (reach == logs.Count)
                        str = string.Format("Emitted {0} ({1}) to all clients", file.Data.ToString(), file.GetSizeReadable());
                    else
                        str = string.Format("Emitted {0} ({1}) to {2} of {3} clients", file.Data.ToString(), file.GetSizeReadable(), reach, logs.Count);
                    Log(str);
                    Notify(verbose >= 2, "Socket-Clipboard", str, StateLog.Normal);
                    break;
                case NotificationType.FailedSent:
                    // ?
                    break;
                case NotificationType.Receiving:
                    if (data == null)
                    {
                        Log("Receiving...");
                        Notify(false, null, null, StateLog.Listen);
                    } else
                        Log(string.Format("Receiving ({0:P})", (float)data), true, true);
                    break;
                case NotificationType.Received:
                    str = string.Format("Received {0} ({1})", file.Data.ToString(), file.GetSizeReadable());
                    Log(str);
                    Notify(verbose >= 2, "Socket-Clipboard", str, StateLog.Normal);
                    break;
                case NotificationType.ClientAdded:
                    Log("Added Client: " + data.ToString());
                    break;
                case NotificationType.Error:
                    Log((((Exception)data).Message));
                    break;
                default:
                    break;
            }
        }


        private void Log(string text, bool timestamp = true, bool append = false)
        {

            if (timestamp)
            {
                text = string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss ff"), text);
            }

            Console.WriteLine(text);
            if(IsHandleCreated)
                Invoke(new Action(() => {
                    LogInner(text, append);
                }));
            else
            {
                LogInner(text, append);
            }
        }

        void LogInner (string text, bool append)
        {
            _status.Text = text;
            if (append && _log.Items.Count > 0)
                _log.Items[_log.Items.Count - 1] = (text);
            else
                _log.SelectedIndex = _log.Items.Add(text);

            UpdateLabels();
        }

  
        void UpdateLabels()
        {
            foreach (ListViewItem item in lvMain.Items)
            {
                if (item.Index == 0)
                    continue;
                var tcp = clients[item.Index - 1];
                string log;
                item.SubItems[1].Text = logs.TryGetValue(tcp.Name, out log) ? log : "...";
            }
        }

        private void Notify(bool notify, string title, string text, StateLog state, ToolTipIcon icon = ToolTipIcon.Info)
        {
            if (IsHandleCreated)
            Invoke(new Action(() =>
            {
                switch (state)
                {

                    case StateLog.Normal:
                        _notify.Icon = Resources.state_normal;
                        break;
                    case StateLog.Busy:
                        _notify.Icon = Resources.state_busy;
                        break;
                    case StateLog.Listen:
                        _notify.Icon = Resources.state_listen;
                        break;
                }
               
            }));

            if (notify)
            {
                _notify.ShowBalloonTip(1000, title, text, icon);
            }
        }

        enum StateLog
        {
            Normal = 0,
            Busy = 1,
            Listen = 2,
            Offline = 3,
        }


    }
}
