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
            int x=0;
            Console.WriteLine("Choose:");
            Console.WriteLine("1.Create folder");
            Console.WriteLine("2.Delete folder");
            Console.WriteLine("3.Create File");
            Console.WriteLine("4.Delete File");
            Console.WriteLine("5.View File");

            x=int.Parse(Console.ReadLine());
            switch (x)
            {
                case 1:
                    //poziv metode za kreiranje foldera
                    break;

                case 2:
                    //poziv metode za brisanje foldera
                    break;

                case 3:
                    //poziv metode za kreiranje fajla
                    break;

                case 4:
                    //poziv metode za brisanje fajla
                    break;
                    
                case 5:
                    //listing
                    break;
            }
        }
        public static void Connect()
        {
            var binding = new NetTcpBinding();
            ChannelFactory<IServices> factory = new
           ChannelFactory<IServices>(binding, new
           EndpointAddress("net.tcp://localhost:6000/CLS"));
            proxy = factory.CreateChannel();
        }
    }
}
