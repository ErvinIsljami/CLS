using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;

namespace CLS
{
    public static class SQLHelper
    {
        static SQLiteConnection m_dbConnection;
        static string myDatabaseFileName = "MyDatabase.sqlite";
        static string databaseName = "Tabela";
        static SQLHelper()
        {
            
                //kreiranje fajla,konekcija i kreiranje tabele
                SQLiteConnection.CreateFile(myDatabaseFileName);

                m_dbConnection = new SQLiteConnection("Data Source=" + myDatabaseFileName + ";Version=3;");
                m_dbConnection.Open();

                string sql = "create table "+ databaseName +" (user varchar(20)," +
                    "method varchar(30)," +
                    "errMsg varchar(90)," +
                    "DBMID varchar(50))";
                //fali postavka za key

                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();
                //m_dbConnection.Close();
        }

        public static void ExecuteCommand(string commands)
        {
            SQLiteCommand command = new SQLiteCommand(commands, m_dbConnection);
            command.ExecuteNonQuery();
        }

        public static string GetSqlCommand(string user, string method, string errorMsg, string dbmid)
        {
            //string ret="";
            string ret = "insert into " + databaseName + " (user,method,errMsg,DBMID) values ('" +
                user + "','" + method + "','" + errorMsg + "', '" + dbmid +"')";

            return ret;
        }
    }
}
