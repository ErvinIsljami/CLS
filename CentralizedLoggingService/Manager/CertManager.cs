using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Manager
{
    public class CertManager
    {
        public static X509Certificate2 GetCertificateFromStorage(StoreName storeName, StoreLocation storeLocation, string subjectName)
        {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certCollection = store.Certificates.Find(X509FindType.FindBySubjectName, subjectName, true);

            /// Check whether the subjectName of the certificate is exactly the same as the given "subjectName"
            foreach (X509Certificate2 c in certCollection)
            {
                if (c.SubjectName.Name.Equals(string.Format("CN={0}", subjectName)))
                {
                    return c;
                }
            }

            return null;
        }
        public static X509Certificate2 GetCertificateFromFile(string fileName)
        {
            X509Certificate2 certificate = null;

            ///In order to create .pfx file, access to a protected .pvk file will be required.
            ///For security reasons, password must not be kept as string. .NET class SecureString provides a confidentiality of a plaintext
            Console.Write("Insert password for the private key: ");
            string pwd = Console.ReadLine();

            ///Convert string to SecureString
            SecureString secPwd = new SecureString();
            foreach (char c in pwd)
            {
                secPwd.AppendChar(c);
            }
            pwd = String.Empty;

            /// try-catch necessary if either the speficied file doesn't exist or password is incorrect
            try
            {
                certificate = new X509Certificate2(fileName, secPwd);
            }
            catch (Exception e)
            {
                Console.WriteLine("Erroro while trying to GetCertificateFromFile {0}. ERROR = {1}", fileName, e.Message);
            }

            return certificate;
        }
    }
}
