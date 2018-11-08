using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface ILogger
    {
        [OperationContract]
        //napravi novy entry za sql i ubaci novu vrednost
        void LogSuccessfulEvent(string user, string method);
 
        [OperationContract]
        void LogErrorEvent(string user, string method, string errorMessage);

        //[OperationContract]
        //void CriticLevel(string errorMessage);
    }
}
