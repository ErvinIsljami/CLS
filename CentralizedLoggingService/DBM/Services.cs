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
        private string path = "..\\..\\..\\root";
        public void CreateNewFile(string name)
        {
            string newPath = path + "\\" + name;

            File.Create(path + "\\" + name);
        }

        public void CreateNewFolder(string name)
        {
            Directory.CreateDirectory(path + "\\" + name);
        }

        public void DeleteFile(string name)
        {
            string newPath = path + "\\" + name;
            if (Uri.IsWellFormedUriString(newPath, UriKind.Absolute))
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

        public string ViewTree()
        {
            int numberOfTabs = 0;
            return PrintTree(this.path, ref (numberOfTabs));
        }

        private string PrintTree(string path, ref int numberOfTabs)
        {
            if (Directory.GetDirectories(path).Count() == 0)
            {
                if (Directory.GetFiles(path).Count() == 0)
                {
                    return "";
                }
            }

            string ret = "";
            foreach (string folder in Directory.GetDirectories(path))
            {
                string[] folders = folder.Split('\\');
                for (int i = 0; i < numberOfTabs; i++)  //print tabs
                    ret += "\t";

                ret += folders[folders.Count() - 1] + "\n"; //print folder name
                numberOfTabs++;
                ret += GetAllFiles(folder, ref numberOfTabs);   //print files in folder
                ret += PrintTree(folder, ref numberOfTabs); //printf subfolders
                numberOfTabs--;

            }

            return ret;
        }
        private string GetAllFiles(string path, ref int numberOfTabs)
        {
            string ret = "";
            foreach (string file in Directory.GetFiles(path))
            {
                string[] files = file.Split('\\');
                for (int i = 0; i < numberOfTabs; i++)
                {
                    ret += "\t";
                }
                ret += files[files.Count() - 1] + "\n";
            }

            return ret;
        }

    }
}
