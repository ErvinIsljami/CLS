using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IServices
    {
        [OperationContract]
        void CreateNewFolder(string name);

        [OperationContract]
        void DeleteFolder(string name);

        [OperationContract]
        string ViewTree();

        [OperationContract]
        void CreateNewFile(string name);

        [OperationContract]
        void DeleteFile(string name);

        [OperationContract]
        void WriteToFile(string name, string text);

    }
}
