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
        [FaultContract(typeof(DatabaseException))]
        void CreateNewFolder(string name);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void DeleteFolder(string name);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        string ViewTree();

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void CreateNewFile(string name);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void DeleteFile(string name);

        [OperationContract]
        [FaultContract(typeof(DatabaseException))]
        void WriteToFile(string name, string text);

    }
}
