using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger l = new Logger();
            l.LogErrorEvent("ervin", "metoda1", "ervinzgreskom");
            l.LogErrorEvent("ervin2", "metoda2", "ervinzgreskom2");

            Console.ReadLine();
        }
    }
}
