using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

namespace Baka_MPlayer
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            Application.ThreadException += ApplicationThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Returns version information (x.x.x.x)
        /// </summary>
        public static string GetVersion()
        {
            return Application.ProductVersion;
        }

        private static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                var ex = (Exception)e.ExceptionObject;
                DumpData(ex.Message, ex.StackTrace);
                var yes = MessageBox.Show("Baka MPlayer ran into a fatal problem!\n\n" +
                                "Detailed information about the error has been saved to \'error_info.txt\'\n" +
                                "Would you like to view the file?",
                                "Uh oh", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes;
                if (yes)
                    Process.Start("error_info.txt");
            }
            catch (Exception) { }
            finally
            {
                Application.Exit();
            }
        }

        public static void ApplicationThreadException(object sender, ThreadExceptionEventArgs e)
        {
            try
            {
                DumpData(e.Exception.Message, e.Exception.StackTrace);
                var yes = MessageBox.Show("Baka MPlayer ran into a problem it couldn't handle!\n\n" +
                                "Detailed information about the error has been saved to \'error_info.txt\'\n" +
                                "Would you like to view the file?",
                                "Uh oh", MessageBoxButtons.YesNo, MessageBoxIcon.Stop) == DialogResult.Yes;
                if (yes)
                    Process.Start("error_info.txt");
            }
            catch (Exception) { }
        }

        public static void DumpData(string msg, string stackTrace)
        {
            using (var file = new System.IO.StreamWriter("error_info.txt", true))
            {
                var contents = new StringBuilder();
                contents.AppendLine("Baka MPlayer Error Info");
                contents.AppendFormat("Version: {0}\r\n\r\n", GetVersion());

                contents.AppendFormat("Generated (UTC): {0}\r\n", DateTime.UtcNow);
                contents.AppendFormat("OSVersion: {0}\r\n", Environment.OSVersion);
                contents.AppendFormat("Is64BitOperatingSystem: {0}\r\n", Functions.OS.IsRunning64Bit());
                contents.AppendFormat("Is64BitProcess: {0}\r\n\r\n", "n/a");

                contents.AppendFormat("Message: {0}\r\n", msg);
                contents.AppendFormat("Stack Trace:\r\n{0}\r\n", stackTrace);
                contents.AppendLine("--------------------------------------------------");

                file.WriteLine(contents);
            }
        }
    }
}
