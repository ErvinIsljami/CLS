using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.IdentityModel.Policy;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.ServiceModel.Security;
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
            ServiceSecurityAuditBehavior newAuditBehavior = new ServiceSecurityAuditBehavior();

            host.Description.Behaviors.Remove(typeof(ServiceDebugBehavior));
            host.Description.Behaviors.Add(new ServiceDebugBehavior() { IncludeExceptionDetailInFaults = false });

            host.Description.Behaviors.Remove<ServiceSecurityAuditBehavior>();
            host.Description.Behaviors.Add(newAuditBehavior);

            List<IAuthorizationPolicy> policies = new List<IAuthorizationPolicy>()
            {
                new MyAuthorizationPolicy()
            };

            host.Authorization.ServiceAuthorizationManager = new MyAuthorizationManager();
            host.Authorization.ExternalAuthorizationPolicies = policies.AsReadOnly();
            host.Authorization.PrincipalPermissionMode = PrincipalPermissionMode.Custom;

           
            return host;
        }

        public static void ConnectToCLS()
        {
            X509Certificate2 srvCert = CertManager.GetCertificateFromStorage(StoreName.TrustedPeople, StoreLocation.LocalMachine, "wcfservice");
            var binding = new NetTcpBinding();
            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;
          //  ChannelFactory<ILogger> factory = new
          //// ChannelFactory<ILogger>(binding, 
           EndpointAddress address=new EndpointAddress(new Uri("net.tcp://localhost:10005/CLS"), new X509CertificateEndpointIdentity(srvCert));

            using (WCFClient proxy = new WCFClient(binding, address))
            {
                /// 1. Communication test
                proxy.TestCommunication();
                Console.WriteLine("TestCommunication() finished. Press <enter> to continue ...");
                Console.ReadLine();
            }
        }
    }
}
