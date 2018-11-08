using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public class MyAuthorizationManager : ServiceAuthorizationManager
    {
        protected override bool CheckAccessCore(OperationContext operationContext)
        {
            bool authorized = false;

            IPrincipal principal = operationContext.ServiceSecurityContext.AuthorizationContext.Properties["Principal"] as IPrincipal;

            string group = string.Format("{0}\\Viewer", Environment.MachineName);

            if (principal.IsInRole(group))
            {
                return true;
            }

            return authorized;
        }
    }
}
