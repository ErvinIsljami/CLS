using Common;
using Manager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public class WCFClient: IWCFContract, IDisposable
    {
        IWCFContract factory;
        ChannelFactory<IWCFContract> proxy;

        public WCFClient(NetTcpBinding binding, EndpointAddress address)
            
        {
            proxy = new ChannelFactory<IWCFContract>(binding, address);
            /// cltCertCN.SubjectName should be set to the client's username. .NET WindowsIdentity class provides information about Windows user running the given process
            string cltCertCN = Manager.Formatter.ParseName(WindowsIdentity.GetCurrent().Name);

            proxy.Credentials.ServiceCertificate.Authentication.CertificateValidationMode = System.ServiceModel.Security.X509CertificateValidationMode.Custom;
            proxy.Credentials.ServiceCertificate.Authentication.CustomCertificateValidator = new ClientCertValidator();
            proxy.Credentials.ServiceCertificate.Authentication.RevocationMode = X509RevocationMode.NoCheck;

            /// Set appropriate client's certificate on the channel. Use CertManager class to obtain the certificate based on the "cltCertCN"
            proxy.Credentials.ClientCertificate.Certificate = CertManager.GetCertificateFromStorage(StoreName.My, StoreLocation.LocalMachine, "wcfclient");

            factory = proxy.CreateChannel();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void TestCommunication()
        {
            try
            {
                factory.TestCommunication();
            }
            catch (Exception e)
            {
                Console.WriteLine("[TestCommunication] ERROR = {0}", e.Message);
            }
        }
    }
}
