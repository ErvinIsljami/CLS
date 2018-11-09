using DBM.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public class MyPrincipal : IPrincipal
    {
        private WindowsIdentity myIdentity;
        private HashSet<Permission> myPermissions = new HashSet<Permission>();
        public MyPrincipal(IIdentity identity)
        {
            myIdentity = identity as WindowsIdentity;

            for (int i = 0; i < myIdentity.Groups.Count; i++)
            {
                try
                {
                    List<Permission> permissionsForGroup = RBACService.GetPermissions(myIdentity.Groups[i].Value);

                    for (int j = 0; j < permissionsForGroup.Count; j++)
                    {
                        myPermissions.Add(permissionsForGroup[j]);
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
                
               
            }
        }

        public bool IsInRole(string role)
        {
            Permission permissionEnum;
            if (Permission.TryParse(role, true, out permissionEnum))
            {
                return myPermissions.Contains(permissionEnum);
            }
            else
            {
                throw new SecurityException(String.Format("User {0} doesn't have role {1} \n", myIdentity.Name, role), (Exception)null);
            }
        }

        public IIdentity Identity => myIdentity;
    }
}
