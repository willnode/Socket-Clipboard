using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace SocketClipboard
{
    public partial class Inviter : Form
    {
        int port;
        
        public Inviter(int port)
        {
            InitializeComponent();
            this.port = port;
            current = this;
            new Thread(new ThreadStart(Execute)).Start();
        }

        void Execute ()
        {
            Thread.Sleep(50); //Wait until the form shown

            try
            {
                var ip = Utility.GetLocalIPAddress();
                var IP = ip.GetAddressBytes();
                var IPs = ip.ToString();
                var packet = new ClipData(DataType.MetaInvitation, new InvitationBuffer() { host = Dns.GetHostName(), ip = IPs });
                var childIP = IP[3];
                motherIP = IPs.Substring(0, IPs.LastIndexOf('.')) + ".";

                for (byte i = 0; i < 255; i++)
                {
                    if (childIP == i)
                        continue;
                    if (IsDisposed)
                        return;
                    {
                        reaching = i;
                        Invoke(new Action(() => UpdateLog()));
                    }
                    IP[3] = i;
                    var p = new IPAddress(IP);
                    var tcp = new TcpClient();
                    var ok = tcp.ConnectAsync(p, port).Wait(100);
                    if (ok)
                    {
                        var g = tcp.GetStream();
                        new BinaryFormatter().Serialize(g, packet);
                        g.Close();
                        {
                            reached++;
                            Invoke(new Action(() => UpdateLog()));
                        }
                    }
                }
                {
                    reaching = 256;
                    Invoke(new Action(() => UpdateLog()));
                }
            }
            catch (Exception)
            {
            }
            
        }

        static public void RespondTo(InvitationBuffer message, int port)
        {
            var ip = Utility.GetLocalIPAddress();
            var IP = ip.GetAddressBytes();
            var IPs = ip.ToString();

            var packet = new ClipData(DataType.MetaInvitation, new InvitationBuffer() { host = Dns.GetHostName(), ip = IPs });

            var tcp = new TcpClient();
            var ok = tcp.ConnectAsync(message.host, port).Wait(100);
            if (ok)
            {
                var g = tcp.GetStream();
                new BinaryFormatter().Serialize(g, packet);
                g.Close();
            }

        }

        string motherIP;
        int reaching, reached, confirm;


        public void IncrementConfirmation ()
        {
            confirm++;
            Invoke(new Action( () => UpdateLog()));
        }

        void UpdateLog ()
        {
            if (reaching == 256)
            {
                if (reached == confirm)
                {
                    if (reached == 0)
                        Log("Done. No other clients are found.", "Reached 0 clients");
                    else
                        Log("Done. " + reached.ToString() + " clients confirmed.", "Reached " + reached.ToString() + " clients");
                    _quit.Text = "OK";
                } else
                    Log("Waiting for confirmations ...", "Reached " + reached.ToString() + " clients, " + confirm.ToString() + " confirmed" );
            } else
                Log("Sending invitation to " + motherIP + reaching.ToString(), "Reached " + reached.ToString() + " clients, " + confirm.ToString() + " confirmed");
        }

        private void Inviter_FormClosing(object sender, FormClosingEventArgs e)
        {
            current = null;
        }

        void Log(string _1, string _2)
        {
            _st1.Text = _1;
            _st2.Text = _2;
        }


        public static Inviter current;
    }
}
