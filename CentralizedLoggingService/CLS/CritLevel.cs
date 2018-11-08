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

    //    public static event EventHandler CheckInventoryEvent;

    //    /// <summary>
    //    /// The main entry point for the application.
    //    /// </summary>
    //    [STAThread]
    //    static void Main()
    //    {
    //        Application.EnableVisualStyles();
    //        Application.SetCompatibleTextRenderingDefault(false);

    //        System.Timers.Timer tmr = new System.Timers.Timer(300000); //Every 5 minutes
    //        tmr.Elapsed += Tmr_Elapsed;

    //        tmr.Start();


    //        Application.Run(new MainForm());

    //        tmr.Stop();
    //    }

    //    private static void Tmr_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
    //    {
    //        OnCheckInventoryEvent();
    //    }

    //    public static void OnCheckInventoryEvent()
    //    {
    //        if (CheckInventoryEvent != null)
    //            CheckInventoryEvent(null, EventArgs.Empty);
    //    }

    }
}
