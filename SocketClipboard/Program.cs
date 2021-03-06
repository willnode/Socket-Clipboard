﻿using System;
using System.IO;
using System.Security.Permissions;
using System.Threading;
using System.Windows.Forms;
using System.Linq;

namespace SocketClipboard
{
    static class Program
    {
        static Mutex mutex = new Mutex(false, "--SocketClipboard Instance--");

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.AllFlags)]
        static void Main()
        {
            if (!mutex.WaitOne(TimeSpan.FromSeconds(3), false))
            {
                MessageBox.Show("Another instance of SocketCopy is already running. Bye!", "Sorry for this", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                Environment.Exit(0);
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            AppDomain.CurrentDomain.UnhandledException += HandleException;

            var args = Environment.GetCommandLineArgs();
            var m = new Main();
            m.Show();
            if (args.Contains("--background"))
                m.Visible = false;
            Application.Run();
        }

        static void HandleException(object sender, UnhandledExceptionEventArgs args)
        {
            Exception ex = (Exception)args.ExceptionObject;
            Console.WriteLine(ex.ToString());
            MessageBox.Show(ex.Message, ex.GetType().ToString(), MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (!args.IsTerminating)
                return;
            if (MessageBox.Show("This software runs to a trouble and need to quit. Restart the software?", "Sorry for this", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(0);
            }
            else
            {
                MessageBox.Show(ex.ToString(), "Useful information to the dev (please screenshot)");
            }
        }
    }
}
