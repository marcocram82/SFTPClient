using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace SFTPClient
{
    public static class SendFileToServer
    {
        public static int Send(string fileName,string host,string user,string pass,string remotePath)
        {
            var connectionInfo = new ConnectionInfo(host, user, new PasswordAuthenticationMethod(user, pass));
            // Upload File
            using (var sftp = new SftpClient(connectionInfo))
            {

                sftp.Connect();
                string s = sftp.WorkingDirectory;
                sftp.ChangeDirectory(string.Concat( @"/", remotePath));
                //sftp.ChangeDirectory("/MyFolder");
                using (var uplfileStream = System.IO.File.OpenRead(fileName))
                {
                    sftp.UploadFile(uplfileStream, fileName, true);
                }
                sftp.Disconnect();
            }
            return 0;
        }

    }
}
