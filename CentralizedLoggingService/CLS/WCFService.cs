using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLS
{
    public class WCFService : IWCFContract
    {
        public void TestCommunication()
        {
            Console.WriteLine("Communication established.");
        }
    }
}
