using System;
using System.Windows.Forms;
using Resources = SocketClipboard.Properties.Resources;
using System.Linq;

namespace SocketClipboard
{
    public enum NotificationType
    {
        Startup = 1,
        //    GoingOnline = 2,
        //  GoingOffline = 4,
        IPUpdated = 8,
        IPOffline = 16,
        PortUpdated = 32,
        Sending = 64,
        Sent = 128,
        FailedReceive = 256,
        Receiving = 512,
        Received = 1024,
        ClientAdded = 2048,
        Error = 0,
        Copyserver = 4096,
    }

    public partial class Main : Form
    {


        public void Log(NotificationType type, object data = null)
        {
            var file = data as ClipBuffer;
            switch (type)
            {
                case NotificationType.Startup:
                    Log("Server online at " + server.LocalEndpoint.ToString());
                    Notify(NotifyFlag.Informative, "Socket-Clipboard is online", "Socket-Clipboard now watches your clipboard", StateLog.Normal);
                    break;
                case NotificationType.IPUpdated:
                    Log("IP updated at " + data.ToString());
                    break;
                case NotificationType.IPOffline:
                    Log("Network disconnected");
                    break;
                case NotificationType.PortUpdated:
                    Log("Port updated at " + data.ToString());
                    break;
                case NotificationType.Sending:
                    Log("Emitting...");
                    Notify(NotifyFlag.Internal, null, null, StateLog.Busy);
                    break;
                case NotificationType.Sent:
                    var reach = clients.Count(x => "Sent" == x.Log);
                    string str;
                    if (reach == 0)
                        str = string.Format("Emit {0} ({1}) failed", file.ToString(), file.GetSizeReadable());
                    else if (config.Solo && reach == 1)
                        str = string.Format("Emitted {0} ({1}) to targeted solo client", file.ToString(), file.GetSizeReadable());
                    else if (reach == clients.Count)
                        str = string.Format("Emitted {0} ({1}) to all clients", file.ToString(), file.GetSizeReadable());
                    else
                        str = string.Format("Emitted {0} ({1}) to {2} of {3} clients", file.ToString(), file.GetSizeReadable(), reach, clients.Count);
                    Log(str);
                    Notify(NotifyFlag.Verbose, "Socket-Clipboard", str, StateLog.Normal);
                    break;
                case NotificationType.Receiving:
                    Log("Receiving...");
                    Notify(NotifyFlag.Internal, null, null, StateLog.Listen);
                    break;
                case NotificationType.FailedReceive:
                    Log(str = "Listening failed: " + data as string);
                    Notify(NotifyFlag.Verbose, "Socket-Clipboard", str, StateLog.Normal);
                    break;
                case NotificationType.Received:
                    Log(str = string.Format("Received {0} ({1})", file.ToString(), file.GetSizeReadable()));
                    Notify(NotifyFlag.Verbose, "Socket-Clipboard", str, StateLog.Normal);
                    break;
                case NotificationType.ClientAdded:
                    Log("Added Client: " + data.ToString());
                    break;
                case NotificationType.Error:
                    Log((((Exception)data).Message));
                    break;
                case NotificationType.Copyserver:
                    if (data != null)
                    {
                        if (data is string)
                        {
                            Log(str = "Copyserver is started on " + (string)data);
                            Notify(NotifyFlag.Verbose, "Socket-Clipboard", str, StateLog.Normal);
                        } else
                        {
                            Log(str = "Copyserver received a GET request");
                        }
                    }
                    else
                        Log("Copyserver is stopped");
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
            if (IsHandleCreated)
                Invoke(new Action(() =>
                {
                    LogInner(text, append);
                }));
            else
            {
                LogInner(text, append);
            }
        }

        void LogInner(string text, bool append)
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
                item.SubItems[1].Text = tcp.Log;
            }
        }

        private void Notify(NotifyFlag notify, string title, string text, StateLog state, ToolTipIcon icon = ToolTipIcon.Info)
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

            if (config.Notify >= notify)
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
