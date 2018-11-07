using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static IServices proxy;
        
        static void Main(string[] args)
        {
            Connect();

            do
            {
                int x = 0;
                Console.WriteLine("Choose:");
                Console.WriteLine("1.Create folder");
                Console.WriteLine("2.Delete folder");
                Console.WriteLine("3.Create File");
                Console.WriteLine("4.Delete File");
                Console.WriteLine("5.View File");

                string name = "luka";
                string name2 = "boskex";

                string name5 = "boske22\\ervin.txt";
                string name3 = "boske22";

                string path = "C:\\Users\\HP\\Desktop\\Projakat\\CLS\\CentralizedLoggingService";

                x = int.Parse(Console.ReadLine());
                switch (x)
                {
                    case 1:
                        proxy.CreateNewFolder(name3);
                        break;

                    case 2:
                        //poziv metode za brisanje foldera
                        break;

                    case 3:
                        proxy.CreateNewFile(name5);
                        break;

                    case 4:
                        //poziv metode za brisanje fajla
                        break;

                    case 5:
                        proxy.ViewTree();
                        break;
                }

            } while (true);

        }
        public static void Connect()
        {
            var binding = new NetTcpBinding();
            ChannelFactory<IServices> factory = new
           ChannelFactory<IServices>(binding, new
           EndpointAddress("net.tcp://localhost:6000/DBM"));
            proxy = factory.CreateChannel();
        }
    }
}
