using System;
using System.Data.SqlClient;
using System.IO;

namespace SFTPClient
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Process Start");


            string[] linhas = File.ReadAllLines("settings.cfg");
            
            String conn = linhas[0];
            String db = linhas[1];
            string backupfile = linhas[2]; 
            string SFTPuser = linhas[3];
            string SFTPpass = linhas[4];
            string SFTPhost = linhas[5];
            string SFTPpath = linhas[6];
            string remoteFile = backupfile;

            ProcessDB(db, conn);

            string path = "";


            if (linhas.Length == 7)
                path = String.Concat(Environment.CurrentDirectory, "\\", backupfile);
            else
                path = linhas[7];

            CreateBackup(db, path, conn);

            if (linhas.Length == 9 && linhas[8].Length > 3)
            {
                remoteFile = backupfile;
                backupfile = linhas[8];
            }
            else
                remoteFile = backupfile;



            SFTPClient.SendFileToServer.Send(backupfile, SFTPhost, SFTPuser, SFTPpass,SFTPpath, remoteFile);
        }

        static void ProcessDB(string DBName,string connString)
        {
            String sql = Resources.Resources.ProcessDB;
            sql = sql.Replace("@@dbname", DBName); 



            ExecuteCommand(sql, connString);

        }
        static void CreateBackup(string DBName,string backupFile, string connString)
        {
            String sql = Resources.Resources.CreateBackup;
            sql = sql.Replace("@@dbname", DBName).Replace("@@backupfile", backupFile);

           ExecuteCommand(sql, connString);

        }

        static void ExecuteCommand(string sql,string connectionString)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            SqlCommand cmd = new SqlCommand(sql, conn);
            conn.Open();
            cmd.ExecuteNonQuery();
        }
    }
}
