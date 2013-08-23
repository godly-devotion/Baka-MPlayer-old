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
    private volatile bool checkSuccessful = false;

    public bool Check(bool isSilent)
    {
        var checkThread = new Thread(check);
        checkThread.Start(isSilent);

        while (checkThread.IsAlive)
            Thread.Sleep(500);

        return checkSuccessful;
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
                throw new Exception("No valid version number was returned.");

            // at this point, checking for updates should have been successful
            checkSuccessful = true;

            if (version.Equals(Application.ProductVersion))
            {
                if (!(bool) isSilent)
                    MessageBox.Show("You have the latest version!", "No Updates Available",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
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
