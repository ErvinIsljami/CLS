using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Program
    {
        public static IServices proxy;
        
        static void Main(string[] args)
        {
            bool isConnected = false;
            do
            {
                Console.WriteLine("Input your port");
                string port = Console.ReadLine();

                try
                {
                    ConnectToDBM(port);
                    Console.WriteLine(proxy.ViewTree());
                    isConnected = true;
                }
                catch
                {
                    Console.WriteLine("You entered wrong port");
                    continue;
                }
            } while (!isConnected);
            

            do
            {
                int x = 0;
                Console.WriteLine("Choose:");
                Console.WriteLine("0.Exit.");
                Console.WriteLine("1.Create folder.");
                Console.WriteLine("2.Delete folder.");
                Console.WriteLine("3.Create File.");
                Console.WriteLine("4.Delete File.");
                Console.WriteLine("5.View File.");
                Console.WriteLine("6.Write to File.");
                Console.WriteLine("7.Edit File.");
                try
                {
                    x = int.Parse(Console.ReadLine());
                }
                catch
                {
                    Console.Clear();
                    Console.WriteLine("Input invalid. Please try again.");
                    continue;
                }
                switch (x)
                {
                    case 0:
                        return;
                    case 1:
                        Console.WriteLine("Input folder you want to create.");
                        string folderCreate = Console.ReadLine();
                        try
                        {
                            proxy.CreateNewFolder(folderCreate);
                        }
                        catch(FaultException<DatabaseException> e)
                        {
                            Console.WriteLine(e.Detail.Reason);
                            //Console.ReadLine();
                        }
                        break;

                    case 2:
                        Console.WriteLine("Input folder you want to delete.");
                        string folderDelete = Console.ReadLine();
                        proxy.DeleteFolder(folderDelete);
                        break;

                    case 3:
                        Console.WriteLine("Input file you want to create.");
                        string fileCreate = Console.ReadLine();
                        try
                        {
                            proxy.CreateNewFile(fileCreate);
                        }
                        catch(FaultException<DatabaseException> dbe)
                        {
                            Console.WriteLine(dbe.Detail.Reason);
                        }
                        break;

                    case 4:
                        Console.WriteLine("Input file you want to delete.");
                        string fileDelete = Console.ReadLine();
                        proxy.DeleteFolder(fileDelete);            
                        break;

                    case 5:
                        proxy.ViewTree();
                        break;

                    case 6:
                        Console.WriteLine("Input path and name of file you want to write to.");
                        string name_and_path = Console.ReadLine();
                        Console.WriteLine("Input text");
                        string text = Console.ReadLine();
                        proxy.WriteToFile(name_and_path, text);
                        break;

                    case 7:
                        Console.WriteLine("Input path and name of file you want to edit.");
                        string name_and_path2 = Console.ReadLine();
                        Console.WriteLine("Input text");
                        string text2 = Console.ReadLine();
                        proxy.WriteToFile(name_and_path2, text2);
                        break;
                    
                }

            } while (true);
        }
        public static void ConnectToDBM(string port)
        {
            var binding = new NetTcpBinding();
            ChannelFactory<IServices> factory = new
           ChannelFactory<IServices>(binding, new
           EndpointAddress("net.tcp://localhost:"+ port +"/DBM"));
            proxy = factory.CreateChannel();
        }
    }
}
