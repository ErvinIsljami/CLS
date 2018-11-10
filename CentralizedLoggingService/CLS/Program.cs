using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.ServiceModel.Security;
using System.Text;
using System.Threading.Tasks;

namespace CLS
{
    class Program
    {
        public static ServiceHost host;
        static int m;
        static string N;
        static int M;

        static void Main(string[] args)
        {
            host = HostServices();
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);
            host.Credentials.ClientCertificate.Authentication.CertificateValidationMode = X509CertificateValidationMode.Custom;
            host.Credentials.ClientCertificate.Authentication.CustomCertificateValidator = new ServiceCertValidator();

            ///If CA doesn't have a CRL associated, WCF blocks every client because it cannot be validated
            host.Credentials.ClientCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            ///Set appropriate service's certificate on the host. Use CertManager class to obtain the certificate based on the "srvCertCN"
            host.Credentials.ServiceCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, srvCertCN);
            do
            {
                try
                {
                    host.Open();
                }
                catch
                {
                    Console.WriteLine("Port alredy taken.");
                    continue;
                }
            } while (false);
            //int LogErrorEvent(string user, string method, string errorMessage)

            N = ConfigurationManager.GetSection("N").ToString();
            M = Int32.Parse(ConfigurationManager.GetSection("M").ToString());

            Logger l = new Logger();
            l.LogErrorEvent("ervin", "metoda1", "ervinzgreskom");
            l.LogErrorEvent("ervin2", "metoda2", "ervinzgreskom2");
            //m = l.LogErrorEvent(user, method, errorMessage);
            m = l.LogErrorEvent("ervin2", "metoda2", "ervinzgreskom2");

            if(m == 1)
                Console.WriteLine("For method: {0} Critical level: LOW");
            else if(m > 1 && m<=M)
                Console.WriteLine("For method: {0} Critical level: MEDIUM");
            else if(m>M)
                Console.WriteLine("For method: {0} Critical level: CRITICAL");
            Console.ReadLine();

            host.Close();
        }

        public static ServiceHost HostServices()
        {
            string srvCertCN = Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            NetTcpBinding binding = new NetTcpBinding();
            string address = "net.tcp://localhost:12005/CLS";

            binding.Security.Transport.ClientCredentialType = TcpClientCredentialType.Certificate;

            ServiceHost host = new ServiceHost(typeof(Logger));
            host.AddServiceEndpoint(typeof(ILogger), binding, address);

            return host;
        }
    }
}
