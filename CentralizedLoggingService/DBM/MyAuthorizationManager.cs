using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    internal class MyAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            
            MyPrincipal pr = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as MyPrincipal;

            string permitName = null;
            string operationName = operationContext.IncomingMessageHeaders.Action.Substring(operationContext.IncomingMessageHeaders.Action.LastIndexOf("/", StringComparison.OrdinalIgnoreCase) + 1);

            switch(operationName)
            {
                case "DeleteFolder":
                    permitName = "ManageFolder";
                    break;
                case "DeleteFile":
                    permitName = "ManageFile";
                    break;
                case "CreateNewFile":
                    permitName = "ManageFile";
                    break;
                case "CreateNewFolder":
                    permitName = "ManageFolder";
                    break;
                default:
                    permitName = "Read";
                    break;
            }
                
            

            return pr.IsInRole(permitName);
        }
    }
}
