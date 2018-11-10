using Common;
using Manager;
using System;
using System.Collections.Generic;
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
