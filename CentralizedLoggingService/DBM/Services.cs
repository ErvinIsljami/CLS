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
        string user = "luka";

        public void CreateNewFile(string name)
        {
            string newPath = path + "\\" + name;

            try
            {
                File.Create(path + "\\" + name);
                Program.proxy.LogSuccessfulEvent(user, "CreateNewFile");
                WriteToEventLog.Instance().LogSuccess(user, "CreateNewFile");
               
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "CreateNewFile", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user,"CreateNewFile", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        public void CreateNewFolder(string name)
        {
            try
            {
                Directory.CreateDirectory(path + "\\" + name);
                Program.proxy.LogSuccessfulEvent(user, "CreateNewFolder");
                WriteToEventLog.Instance().LogSuccess(user, "CreateNewFolder");
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "CreateNewFolder", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user, "CreateNewFolder", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        public void DeleteFile(string name)
        {
            string newPath = path + "\\" + name;

            try
            {
                if (Uri.IsWellFormedUriString(newPath, UriKind.Absolute))
                {
                    File.Delete(path + "\\" + name);
                    WriteToEventLog.Instance().LogSuccess(user, "DeleteFile");
                    Program.proxy.LogSuccessfulEvent(user, "DeleteFile");
                }
                else
                {
                    throw new UriFormatException("Path not valid. The path " + "was not found or well formated");
                }
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "DeleteFile", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user, "DeleteFile", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        public void DeleteFolder(string name)
        {
            try
            {
                Directory.Delete(path + "\\" + name);
                Program.proxy.LogSuccessfulEvent(user, "DeleteFolder");
                WriteToEventLog.Instance().LogSuccess(user, "DeleteFolder");

            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "DeleteFolder", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user, "DeleteFolder", "error");
                throw new Exception(e.Message.ToString());
            }
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
