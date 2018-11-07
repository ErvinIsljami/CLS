using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public class Services : IServices
    {
        string path = "C:\\Users\\HP\\Desktop\\Projakat\\CLS\\CentralizedLoggingService";

        public void CreateNewFile(string name) 
        {
            string newPath = path + "\\" + name;

            File.Create(path + "\\" + name);
        }

        public void CreateNewFolder(string name)
        {
            Directory.CreateDirectory(path + "\\"+name);
        }

        public void DeleteFile(string name)
        {
            string newPath = path + "\\" + name;
            if(Uri.IsWellFormedUriString(newPath, UriKind.Absolute))
            {
                File.Delete(path + "\\" + name);
            }
            else
            {
                throw new UriFormatException("Path not valid. The path " + "was not found or well formated");
            }
        }

        public void DeleteFolder(string name)
        {
            Directory.Delete(path + "\\" + name);
        }

        public void ViewTree()
        {
            string[] entries = Directory.GetFileSystemEntries(path, "*", SearchOption.AllDirectories);

            string[] names = Directory.GetDirectories(path);

            Directory.GetParent(path).ToString();

            for (int i = 0; i < entries.Count(); i++)
            {
                Console.WriteLine(entries[i].Substring(path.Count()) + "\n");
            }
        }



    }
}
