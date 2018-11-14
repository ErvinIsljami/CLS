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
    public class Services : IServices, IWCFContract
    {
        private string path = "..\\..\\..\\root";
        public static string user = null;

        public static string NameOfUser()
        {
            string ret;
            IIdentity id = Thread.CurrentPrincipal.Identity;
            WindowsIdentity Identity = id as WindowsIdentity;

            Console.WriteLine("Name of user" + Identity.Name);
            ret = Identity.Name;

            return ret;
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFile")]
        public void CreateNewFile(string name)
        {
            string newPath = path + "\\" + name;

            try
            {
                if (File.Exists(newPath))
                {
                    DatabaseException db = new DatabaseException();
                    db.Reason = "File alredy exists.";
                    throw new FaultException<DatabaseException>(db);
                }
                File.Create(newPath);
                //Program.proxy.LogSuccessfulEvent(user, "CreateNewFile");
                DecideForSuccessful(Program.type, "CreateNewFile");
            }
            catch (Exception e)
            {
                //Program.proxy.LogErrorEvent(user, "CreateNewFile", e.Message.ToString());
                DecideForUnSuccessful(Program.type, "CreateNewFile", e.Message);
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void CreateNewFolder(string name)
        {
            try
            {
                Directory.CreateDirectory(path + "\\" + name);
               // Program.proxy.LogSuccessfulEvent(user, "CreateNewFolder");
                DecideForSuccessful(Program.type, "CreateNewFolder");
            }
            catch (Exception e)
            {
               // Program.proxy.LogErrorEvent(user, "CreateNewFolder", e.Message.ToString());
                DecideForUnSuccessful(Program.type, "CreateNewFile", e.Message);
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
                    DecideForSuccessful(Program.type, "DeleteFile");
                    //Program.proxy.LogSuccessfulEvent(user, "DeleteFile");
                }
                else
                {
                    throw new UriFormatException("Path not valid. The path " + "was not found or well formated");
                }
            }
            catch (Exception e)
            {
                //Program.proxy.LogErrorEvent(user, "DeleteFile", e.Message.ToString());
                DecideForUnSuccessful(Program.type, "DeleteFile", e.Message);
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void DeleteFolder(string name)
        {
            try
            {
                Directory.Delete(path + "\\" + name);
                //Program.proxy.LogSuccessfulEvent(user, "DeleteFolder");
                DecideForSuccessful(Program.type, "DeleteFolder");
            }
            catch (Exception e)
            {
               // Program.proxy.LogErrorEvent(user, "DeleteFolder", e.Message.ToString());
                DecideForUnSuccessful(Program.type, "DeleteFolder", e.Message);
                throw new Exception(e.Message.ToString());
            }
        }

        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "ManageFolder")]
        public void WriteToFile(string name, string text)
        {
            string file_path = path + "\\" + name;

            if (File.Exists(file_path))
            {
                DatabaseException e = new DatabaseException();
                e.Reason = "File doesn't exist";
                throw new FaultException<DatabaseException>(e);

            }
            try
            {
                using (StreamWriter outputFile = new StreamWriter(file_path))
                {
                    outputFile.Write(text);
                    //Program.proxy.LogSuccessfulEvent(user, "WriteToFile");
                    DecideForSuccessful(Program.type, "WriteToFile");
                }
            }
            catch (Exception e)
            {
                //Program.proxy.LogErrorEvent(user, "WriteToFile", e.Message.ToString());
                DecideForUnSuccessful(Program.type, "WriteToFile", e.Message);
            }

        }


        [PrincipalPermission(SecurityAction.Demand, Authenticated = true, Role = "Read")]
        public string ViewTree()
        {
            user = NameOfUser();
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
                    ret += "  ";

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
                    ret += "  ";
                }
                
                ret += "└" + files[files.Count() - 1] + "\n";
            }

            return ret;
        }

        public void DecideForSuccessful(LoggingType type, string method)
        {
            if (Program.type.ToString() == "WriteToEventLog")
                WriteToEventLog.Instance().LogSuccess(user, method);
            else if (Program.type.ToString() == "WriteToXml")
                WriteToXml(user, method);
            else
                Console.WriteLine("WrongChoice");
        }

        public void DecideForUnSuccessful(LoggingType type, string method, string error)
        {
            if (Program.type.ToString() == "WriteToEventLog")
                WriteToEventLog.Instance().LogFailure(user, method, "error");
            else if (Program.type.ToString() == "WriteToXml")
                WriteToXml(user, method);
            else
                Console.WriteLine("WrongChoice");
        }

        public void WriteToXml(string user, string file2)
        {
            string text = string.Format(" User {0} succesfully accessed {1}", user, file2);

            var path = @"events.xml";

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
