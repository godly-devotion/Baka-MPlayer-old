/******************************************
* UpdateChecker (by Joshua Park & u8sand) *
******************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using Baka_MPlayer.Forms;

public class UpdateInfo
{
    public bool UpdateAvailable;
    public string LatestVer;
    public string Date;
    public string BugFixes;

    public UpdateInfo(bool updateAvailable, string latestVer, string date, string bugFixes)
    {
        this.UpdateAvailable = updateAvailable;
        this.LatestVer = latestVer;
        this.Date = date;
        this.BugFixes = bugFixes;
    }
}

public class UpdateChecker
{
    private const string website = "bakamplayer.u8sand.net";
    private const string versionPath = "/version";

    public void Check(bool isSilent)
    {
        var checkThread = new Thread(check);
        checkThread.Start(isSilent);
    }

    private void check(object isSilent)
    {
        try
        {
            Socket client = null;

            IPHostEntry host = Dns.GetHostEntry(website);
            foreach (IPAddress address in host.AddressList)
            {
                client = new Socket(address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                var remoteEP = new IPEndPoint(address, 80);
                client.Connect(remoteEP);
                break;
            }
            client.Send(
                Encoding.ASCII.GetBytes("GET " + versionPath + " HTTP/1.0\r\nHost: " + website +
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
                while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                {
                    var i = line.IndexOf('=');
                    if (i != -1)
                    {
                        var type = line.Substring(0, i);
                        if (type.Equals("version", StringComparison.Ordinal))
                            version = line.Substring(i + 1);
                        else if (type.Equals("date", StringComparison.Ordinal))
                            date = line.Substring(i + 1);
                        else if (type.Equals("bugfixes", StringComparison.Ordinal))
                        {
                            bugfixes = line.Substring(i + 1);
                            while (!string.IsNullOrEmpty(line = reader.ReadLine()))
                                bugfixes = bugfixes + '\n' + line;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(version))
                throw new Exception("No valid version number was returned.");

            if (version.Equals(Application.ProductVersion, StringComparison.Ordinal))
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
            if (!(bool)isSilent && MessageBox.Show("We couldn't check for updates.\nDetails: " + e.Message,
                "Cannot Check for Updates", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK) { }
        }
    }
}
