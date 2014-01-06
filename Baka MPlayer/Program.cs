using Baka_MPlayer.Forms;
using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

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
        /// Returns version information (1.2.3.4)
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
                MessageBox.Show("Baka MPlayer ran into a fatal problem!\n" +
                                "Information about the error has been saved to \'error_info.txt\'",
                                "Uh oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
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
                MessageBox.Show("Baka MPlayer ran into a problem it couldn't handle!\n" +
                                "Information about the error has been saved to \'error_info.txt\'",
                                "Uh oh", MessageBoxButtons.OK, MessageBoxIcon.Stop);
            }
            catch (Exception) { }
        }

        public static void DumpData(string msg, string stackTrace)
        {
            using (var file = new System.IO.StreamWriter("error_info.txt", true))
            {
                var contents = new StringBuilder();
                contents.AppendLine("Baka MPlayer Error Info");
                contents.AppendFormat("Version: {0}\n\n", GetVersion());

                contents.AppendFormat("Generated: {0}\n", DateTime.UtcNow);
                contents.AppendFormat("OSVersion: {0}\n", Environment.OSVersion);
                contents.AppendFormat("Is64BitOperatingSystem: {0}\n", Functions.OS.IsRunning64Bit());
                contents.AppendFormat("Is64BitProcess: {0}\n\n", "n/a");

                contents.AppendFormat("Message: {0}\n", msg);
                contents.AppendFormat("Stack Trace:\n{0}\n", stackTrace);
                contents.AppendLine("--------------------------------------------------");

                file.WriteLine(contents);
            }
        }
    }
}
