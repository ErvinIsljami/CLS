using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Configuration;

namespace CLS
{
    public static class SQLHelper
    {
        static SQLiteConnection m_dbConnection;
        static string myDatabaseFileName = "MyDatabase.sqlite";
        static string databaseName = "Tabela";
        static DateTime timenow;
        static DateTime timemin;
        static string N;
        static int M;

        static SQLHelper()
        {
            SQLiteConnection.CreateFile(myDatabaseFileName);

            m_dbConnection = new SQLiteConnection("Data Source=" + myDatabaseFileName + ";Version=3;");
            m_dbConnection.Open();

            string sql = "create table " + databaseName + " (user varchar(20)," +
                "method varchar(30)," +
                "errMsg varchar(90)," +
                "DBMID varchar(50)," +
                "DateTime datetime," +
                "PRIMARY KEY (DBMID))";

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
            command.ExecuteNonQuery();

            N = ConfigurationManager.GetSection("N").ToString();
            M = Int32.Parse(ConfigurationManager.GetSection("M").ToString());
        }
        

        public static void ExecuteCommand(string commands)
        {
            SQLiteCommand command = new SQLiteCommand(commands, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public static string GetSqlCommand(string user, string method, string errorMsg, string dbmid)
        {
            StringBuilder msg = new StringBuilder(errorMsg.Count());
            for(int i = 0; i < errorMsg.Count();i++)
            {
                if (errorMsg[i] != '\'')
                    msg.Append(errorMsg[i]);
            }
            string ret = "insert into " + databaseName + " (user,method,errMsg,DBMID,DateTime) values ('" +
                user + "','" + method + "','" + msg.ToString() + "', '" + dbmid +"','"+timenow.ToString() +"')";

            return ret;
        }
        public static int GetCritSqlCommand(string user, string method, string errorMsg)
        {
            // time = 2/30
            string time = N;
            string[] minutes = time.Split('/');
            timemin = DateTime.Now;
            timenow = DateTime.Now;



            timemin = timemin.AddMinutes((Int32.Parse(minutes[0])) * -1);
            timemin = timemin.AddSeconds((Int32.Parse(minutes[1])) * -1);
            string crtlvl = "SELECT * (DISTINCT " + method + ") FROM " + databaseName + 
                " WHERE " + errorMsg + " IS NOT NULL AND DateTime >= '"+timemin.ToString()+"' AND DateTime <= '"+timenow.ToString()+"';";
            SQLiteCommand command = new SQLiteCommand(crtlvl, m_dbConnection);

            int a = command.ExecuteNonQuery();
            //a ti vraca broj redova rezultata commande

            return a;
            
        }
        
    }
}

/* N = 3;
 * M = 2;
 * ervin write cannotwrite 12:00 - low
 * ervin write cannotwrite 12:02 - medium
 * ervin write cannotwrite 12:02 - medium
 * ervin write cannotwrite 12:03 - critical
 * 
 * 
 */
