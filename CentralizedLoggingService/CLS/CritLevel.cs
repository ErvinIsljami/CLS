using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS
{
    public class CritLevel
    {
        //    //implementirati nivo kriticnosti svakog dogadjaja
        //    //low evel - detekcija neuspesnog pokusaja nad odredjenim fajlom
        //    //medium level - u periodu od N sekundi/minuta detektuje M puta neuspesan dogadjaj od bilo kog cls klijenta
        //    //critical level - za N min/sec detektuje M+1 neuspesan pokusaj pristupanja od bilo kog cls klijenta

        string tablename = "Tabela";

        

        public void CallSQLWithError(string user, string method, string errorMessage)
        {
            string crtlvl = "SELECT * FROM " + tablename + " WHERE " + errorMessage + " IS NOT NULL;";
            //string sql = SQLHelper.GetSqlCommand(user, method, errorMessage, "DBMID");
            SQLHelper.ExecuteCommand(crtlvl);
            
            //foreach (DataRow dataRow in Table.Rows)
            //{
            //    foreach (var item in dataRow.ItemArray)
            //    {
            //        Console.WriteLine(item);
            //    }
            //}
        }
    }
}
