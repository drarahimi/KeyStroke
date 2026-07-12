using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace KeyStroke
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new Mutex(false, "Global\\KeyStrokeApp_" + Application.ProductName))
            {
                if (!mutex.WaitOne(0, false))
                {
                    MessageBox.Show("KeyStroke is already running.", "Instance Error");
                    return;
                }

                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
                Application.ThreadException += (sender, e) => ReportUnhandledException(e.Exception);
                AppDomain.CurrentDomain.UnhandledException += (sender, e) => ReportUnhandledException(e.ExceptionObject as Exception);

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.SetDefaultFont(new Font(new FontFamily("Segoe UI"), 12f));
                Application.Run(new frmMain());
            }

        }

        private static void ReportUnhandledException(Exception ex)
        {
            MessageBox.Show(
                $"KeyStroke hit an unexpected error and will keep running:\n\n{ex?.Message}",
                "KeyStroke - Unexpected Error",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
        }
    }
}
