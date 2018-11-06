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
        void CreateNewFolder();

        [OperationContract]
        void DeleteFolder();

        [OperationContract]
        void ViewTree();

        [OperationContract]
        void CreateNewFile();

        [OperationContract]
        void DeleteFile();

    }
}
