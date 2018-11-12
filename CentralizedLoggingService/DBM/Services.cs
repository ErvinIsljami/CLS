//FileInfo finfo = new FileInfo(newPath);
//FileSecurity fsecurity = finfo.GetAccessControl();
////also tried it like this //fsecurity.ResetAccessRule(new FileSystemAccessRule(string.Format(@"{0}\{1}", Environment.UserDomainName.ToString(), Environment.UserDomainName.ToString()), FileSystemRights.FullControl, AccessControlType.Allow));
//fsecurity.SetOwner(WindowsIdentity.GetCurrent().User.AccountDomainSid);
//finfo.SetAccessControl(fsecurity);
//Console.WriteLine("OWWWWWWWWWWWNEEEEEEEEEEEER"+finfo.GetAccessControl().GetOwner(typeof(SecurityIdentifier)).Value);

using Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Permissions;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DBM
{
    public class Services : IServices
    {
        public static WindowsIdentity identity = null;
        private string path = "..\\..\\..\\root";
        string user = "luka";

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFile")]
        public void CreateNewFile(string name)
        {
            string newPath = path + "\\" + name;

            try
            {
                if(File.Exists(newPath))
                {
                    DatabaseException db = new DatabaseException();
                    db.Reason = "File alredy exists.";
                    throw new FaultException<DatabaseException>(db);
                }
                File.Create(newPath);

                Program.proxy.LogSuccessfulEvent(user, "CreateNewFile");
                DecideForSuccessful(Program.type);
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "CreateNewFile", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user,"CreateNewFile", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void CreateNewFolder(string name)
        {
            try
            {
                
                Directory.CreateDirectory(path + "\\" + name);
                Program.proxy.LogSuccessfulEvent(user, "CreateNewFolder");
                DecideForSuccessful(Program.type);
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "CreateNewFolder", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user, "CreateNewFolder", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFile")]
        public void DeleteFile(string name)
        {
            string newPath = path + "\\" + name;

            try
            {
                if (Uri.IsWellFormedUriString(newPath, UriKind.Absolute))
                {
                    File.Delete(path + "\\" + name);
                    DecideForSuccessful(Program.type);
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

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void DeleteFolder(string name)
        {
            try
            {
                Directory.Delete(path + "\\" + name);
                Program.proxy.LogSuccessfulEvent(user, "DeleteFolder");
                DecideForSuccessful(Program.type);
            }
            catch (Exception e)
            {
                Program.proxy.LogErrorEvent(user, "DeleteFolder", e.Message.ToString());
                WriteToEventLog.Instance().LogFailure(user, "DeleteFolder", "error");
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void WriteToFile(string name, string text)
        {
            string file_path = path + "\\" + name;

            using (StreamWriter outputFile = new StreamWriter(file_path))
            {
                outputFile.Write(text);
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

        public void DecideForSuccessful(LoggingType type)
        {
            if (Program.type.ToString() == "WriteToEventLog")
                WriteToEventLog.Instance().LogSuccess(user, "CreateNewFile");
            else if (Program.type.ToString() == "WriteToXml")
                WriteToXml(user, "CreateNewFile");
            else
                Console.WriteLine("WrongChoice");
        }

        public void WriteToXml(string user, string file2)
        {
            string text = string.Format(" User {0} succesfully accessed {1}", user, file2);
           
            var path = @"C:\Users\Ervin\Desktop\CLS\CentralizedLoggingService\DBM\bin\Debug\events.xml";

            if (System.IO.File.Exists(path)) //Decides if the player has a xml file already
            {
                //Get data from existing
                XDocument file = XDocument.Load(path);
                XElement winTemp = new XElement("playerWin", user);
               
                ////delete existing file
                //File.Delete(Path.Combine(path));

                ////Creates new file using last game played.
                XDeclaration _obj = new XDeclaration("1.0", "utf-8", "");
                XNamespace gameSaves = "gameSaves";
                XElement fileNew = new XElement("Root",
                                    new XElement("Player",
                                        new XElement("playerName", file2)
                                        ));

                file.Save(path);
            }
            else //if the player doesn't have a txt file it creates one here
            {
                XDeclaration _obj = new XDeclaration("1.0", "utf-8", "");
                XNamespace gameSaves = "gameSaves";
                XElement file = new XElement("Root",
                                    new XElement("Player",
                                        new XElement("playerName", file2)
                                        ));

                file.Save(path);
                Console.WriteLine("Save created: " + path);
            }
        }
    }
}
