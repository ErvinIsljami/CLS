using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IWCFContract
    {
        [OperationContract]
        void TestCommunication();

        //[OperationContract]
        //void SendMessage(string message, byte[] sign)
    }
}
