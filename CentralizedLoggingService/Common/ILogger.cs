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
        void SetLoggingMethod(string user, LoggingType type);

        [OperationContract]
        void LogSuccessfulEvent(string user, string method);
 
        [OperationContract]
        void LogErrorEvent(string user, string method, string errorMessage);

    }
}
