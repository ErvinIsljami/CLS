using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    class Program
    {
        public static ILogger proxy;
        public static ServiceHost host;
        public static LoggingType type;
        static void Main(string[] args)
        {
            bool isConnected = false;
            do
            {
                try
                {
                    ConnectToCLS();
                    isConnected = true;
                    Console.WriteLine("Bole se povezao na Gole");
                }
                catch
                {
                    Console.WriteLine("You entered wrong port");
                    continue;
                }
            } while (!isConnected);

            do
            {
                Console.WriteLine("Choose type of logging");
                Console.WriteLine("1. XML.");
                Console.WriteLine("2. Event log.");
                int choice = Int32.Parse(Console.ReadLine());
                if(choice == 1)
                {
                    type = LoggingType.WriteToXml;
                    break;
                }
                if(choice == 2)
                {
                    type = LoggingType.WriteToEventLog;
                    break;
                }
            } while (true);
            
            do
            {
                Console.WriteLine("Input your port");
                string port = Console.ReadLine();

                try
                {
                    host = HostServices(port);
                    host.Open();
                }
                catch
                {
                    Console.WriteLine("Port alredy taken.");
                    continue;
                }
            } while (false);

            Console.WriteLine("Services is opened. Press <enter> to finish...");
            Console.ReadLine();

            host.Close();
        }

        public static ServiceHost HostServices(string port)
        {
            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:"+port+"/DBM";

            ServiceHost host = new ServiceHost(typeof(Services));
            host.AddServiceEndpoint(typeof(IServices), binding, address);
           // host.Authorization.ServiceAuthorizationManager = new MyAuthorizationManager();
            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = true });

            return host;
        }

        public static void ConnectToCLS()
        {
            var binding = new NetTcpBinding();
            ChannelFactory<ILogger> factory = new
           ChannelFactory<ILogger>(binding, new
           EndpointAddress("net.tcp://localhost:12005/CLS"));
            proxy = factory.CreateChannel();
        }
    }
}
