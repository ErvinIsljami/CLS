using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
namespace CLS
{
    public class Logger : ILogger
    {
        private string N;
        private int M;

        public Logger()
        {
            N = System.Configuration.ConfigurationManager.AppSettings["N"];
            M = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["M"]);
        }
        public int LogErrorEvent(string user, string method, string errorMessage)
        {
            int crt = SQLHelper.GetCritcalLevel(method, errorMessage, N, M);
            if(crt > 4)
            {
                Console.WriteLine("*********ALARM**********");
            }

            string sql = SQLHelper.GetSqlCommand(user, method, errorMessage, "DBMID");
            SQLHelper.ExecuteCommand(sql);

            return crt;
        }

        public void LogSuccessfulEvent(string user, string method)
        {
            string sql = SQLHelper.GetSqlCommand(user, method, "NULL", "DBMID");
            SQLHelper.ExecuteCommand(sql);
        }
        //public void TestCommunication()
        //{
        //    Console.WriteLine("Communication established.");
        //}

        //*******************dodati user i pass preko koda**********
        #region adduser
        /*
        public void AddUser()
        {
            try
            {
                DirectoryEntry AD = new DirectoryEntry("WinNT://" +
                                    Environment.MachineName + ",computer");
                DirectoryEntry NewUser = AD.Children.Add("TestUser1", "user");
                NewUser.Invoke("SetPassword", new object[] { "#12345Abc" });
                NewUser.Invoke("Put", new object[] { "Description", "Test User from .NET" });
                NewUser.CommitChanges();
                DirectoryEntry grp;

                grp = AD.Children.Find("Guests", "group");
                if (grp != null) { grp.Invoke("Add", new object[] { NewUser.Path.ToString() }); }
                Console.WriteLine("Account Created Successfully");
                Console.ReadLine();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.ReadLine();

            }
        }*/
        #endregion
    }
}
