/*
 * Update Checker
 * 
 * Copyright (c) 2014, Joshua Park & u8sand
 */

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

namespace Baka_MPlayer.Updates
{
    public class UpdateChecker
    {
        private const string Website = "bakamplayer.u8sand.net";
        private const string VersionPath = "/version";

        public void Check(bool isSilent)
        {
            var checkThread = new Thread(check);
            checkThread.Start(isSilent);
        }

        private static void check(object isSilent)
        {
            try
            {
                Socket client = null;

                IPHostEntry host = Dns.GetHostEntry(Website);
                foreach (IPAddress address in host.AddressList)
                {
                    client = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    var remoteEP = new IPEndPoint(address, 80);
                    client.Connect(remoteEP);
                    break;
                }
                client.Send(
                    Encoding.ASCII.GetBytes("GET " + VersionPath + " HTTP/1.0\r\nHost: " + Website +
                                            "\r\nConnection: Close\r\n\r\n"));

                var recvd = new byte[1024];
                var data = new StringBuilder();
                int recvdBytes = client.Receive(recvd);
                while (recvdBytes != 0)
                {
                    data.Append(Encoding.ASCII.GetChars(recvd), 0, recvdBytes);
                    recvdBytes = client.Receive(recvd);
                }
                client.Close();

                var version = string.Empty;
                var date = string.Empty;
                var bugfixes = string.Empty;

                using (var reader = new StringReader(data.ToString()))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        var i = line.IndexOf('=');
                        if (i != -1)
                        {
                            var type = line.Substring(0, i);
                            if (type.Equals("version"))
                                version = line.Substring(i + 1);
                            else if (type.Equals("date"))
                                date = line.Substring(i + 1);
                            else if (type.Equals("bugfixes"))
                            {
                                bugfixes = line.Substring(i + 1);
                                while ((line = reader.ReadLine()) != null)
                                    bugfixes = bugfixes + '\n' + line;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(version))
                    throw new Exception("A valid version number was not returned.");

                if (version.Equals(Program.GetVersion()))
                {
                    if (!(bool) isSilent)
                    {
                        MessageBox.Show("You have the latest version!", "No Updates Available",
                            MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                    }
                    return;
                }

                var form = new UpdateForm(new UpdateInfo(true, version, date, bugfixes));
                if (form.ShowDialog() == DialogResult.Cancel) { }
            }
            catch (Exception e)
            {
                if (!(bool)isSilent && MessageBox.Show("We couldn't check for updates.\n\nDetails: " + e.Message,
                    "Cannot Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) { }
            }
        }
    }
}
