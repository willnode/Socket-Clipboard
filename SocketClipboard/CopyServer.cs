using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SocketClipboard
{
    // https://codehosting.net/blog/BlogEngine/post/Simple-C-Web-Server
    public class WebServer : IDisposable
    {
        private readonly HttpListener _listener = new HttpListener();
        private readonly Action<HttpListenerRequest, HttpListenerResponse> _responderMethod;

        public WebServer(string[] prefixes, Action<HttpListenerRequest, HttpListenerResponse> method)
        {
            if (!HttpListener.IsSupported)
                throw new NotSupportedException(
                    "Needs Windows XP SP2, Server 2003 or later.");

            // URI prefixes are required, for example 
            // "http://localhost:8080/index/".
            if (prefixes == null || prefixes.Length == 0)
                throw new ArgumentException("prefixes");

            foreach (string s in prefixes)
                _listener.Prefixes.Add(s);

            _responderMethod = method ?? throw new ArgumentException("method");

        }

        public WebServer(Action<HttpListenerRequest, HttpListenerResponse> method, params string[] prefixes)
            : this(prefixes, method) { }

        public void Dispose()
        {
            if (_listener.IsListening)
                _listener.Stop();
            _listener.Close();
        }

        public bool IsRunning { get { return _listener.IsListening; } }
        public void Run()
        {
            _listener.Start();
            ThreadPool.QueueUserWorkItem((o) =>
            {
                Console.WriteLine("Webserver running...");
                Thread.Sleep(100);
                try
                {
                    while (_listener.IsListening)
                    {
                        ThreadPool.QueueUserWorkItem((c) =>
                        {
                            var ctx = c as HttpListenerContext;
                            try
                            {
                                _responderMethod(ctx.Request, ctx.Response);
                            }
                            catch { } // suppress any exceptions
                            finally
                            {
                                // always close the stream
                                ctx.Response.OutputStream.Close();
                            }
                        }, _listener.GetContext());
                    }
                }
                catch { } // suppress any exceptions
            });
        }

        public void Stop()
        {
            _listener.Stop();
        }
    }

    public class CopyServer
    {

        int port;

        public CopyServer(int port)
        {
            this.port = port;
            server = new WebServer(Response, URL);
        }

        public WebServer server;

        void Response(HttpListenerRequest request, HttpListenerResponse response)
        {
            var buf = File.ReadAllBytes(Application.ExecutablePath);
            response.AddHeader("Content-disposition", "attachment; filename=" + Path.GetFileName(Application.ExecutablePath));
            response.ContentType = "application/x-msdownload";
            response.ContentLength64 = buf.Length;
            response.OutputStream.Write(buf, 0, buf.Length);
            response.StatusCode = (int)HttpStatusCode.OK;
            Main.Current.Log(NotificationType.Copyserver, 0);
        }

        string URL {  get { return "http://*:" + port + "/"; } }

        string URLWithIP { get { return "http://" + Utility.GetLocalIPAddress().ToString() +":" + port + "/"; } }

        public void SetRun(bool run)
        {
            if (run == server.IsRunning) return;
            if (run)
            {
                if (!Utility.HaveAdminAccess)
                {
                    if (MessageBox.Show("Starting server across LAN requires an admin access so we can bypass firewall. Restart as administrator?", "Permission Grant", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                        Utility.HaveAdminAccess = true;
                }
                Process.Start("netsh", "http add urlacl url=" + URL + " user=Everyone listen=yes").WaitForExit();
                Process.Start("netsh", "advfirewall firewall add rule name=\"SocketClipboard Grant\" dir=in action=allow protocol=TCP localport=" + port.ToString()).WaitForExit();
                Process.Start("netsh", "advfirewall firewall add rule name=\"SocketClipboard Grant\" dir=out action=allow protocol=TCP localport=" + port.ToString()).WaitForExit();
                server.Run();
                Main.Current.Log(NotificationType.Copyserver, URLWithIP);
            }
            else
            {
                Process.Start("netsh", "http delete urlacl url=" + URL + "").WaitForExit();
                server.Stop();
                Main.Current.Log(NotificationType.Copyserver, null);

            }
        }
    }
}
