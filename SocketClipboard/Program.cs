using System;
using System.IO;
using System.Security.Permissions;
using System.Windows.Forms;

namespace SocketClipboard
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.AllFlags)]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += HandleException;
            CheckLog();

            Application.Run(new Form1());
        }

        static void CheckLog ()
        {
            var file = new FileInfo("Log.txt");
            if (file.Exists)
            {
                Console.SetOut(file.CreateText());
            }
        }
        static void HandleException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            Console.WriteLine(ex.ToString());
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
