using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace CLS
{
    class Program
    {
        public static ServiceHost host;

        static void Main(string[] args)
        {
         
            do
            {
                try
                {
                    host = HostServices();
                    host.Open();
                }
                catch
                {
                    Console.WriteLine("Port alredy taken.");
                    continue;
                }
            } while (false);

            Logger l = new Logger();
            l.LogErrorEvent("ervin", "metoda1", "ervinzgreskom");
            l.LogErrorEvent("ervin2", "metoda2", "ervinzgreskom2");

            Console.WriteLine("Services is opened. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();
        }

        public static ServiceHost HostServices()
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:12005/CLS";

            ServiceHost host = new ServiceHost(typeof(Logger));
            host.AddServiceEndpoint(typeof(ILogger), binding, address);

            return host;
        }
    }
}
