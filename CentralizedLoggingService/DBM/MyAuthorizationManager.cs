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
            return pr.IsInRole("Read");
        }
    }
}
