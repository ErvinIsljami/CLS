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
        string databaseName = "Tabela";
        public void LogErrorEvent(string user, string method, string errorMessage)
        {
            string sql = SQLHelper.GetSqlCommand(user, method, errorMessage, "DBMID");
            SQLHelper.ExecuteCommand(sql);
        }

        public void LogSuccessfulEvent(string user, string method)
        {
            string sql = SQLHelper.GetSqlCommand(user, method, "NULL", "DBMID");
            SQLHelper.ExecuteCommand(sql);
        }

        
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
