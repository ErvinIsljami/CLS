using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace DBM.RBAC
{
    public enum Permission
    {
        Read,
        ManageFile,
        ManageFolder
    }

    class RBACService : IDisposable
    {
        private static readonly Dictionary<string, HashSet<Permission>> GroupPermissionsMapping = new Dictionary<string, HashSet<Permission>>();
        private static PrincipalContext context = new PrincipalContext(ContextType.Machine);
        private static Dictionary<string, List<Permission>> configuration;
        public static bool CheckPermission(string group, Permission permission)
        {
            HashSet<Permission> groupPermissions;
            if (GroupPermissionsMapping.TryGetValue(group, out groupPermissions))
            {
                return groupPermissions.Contains(permission);
            }
            else
            {
                return false;
            }
        }

        public static List<Permission> GetPermissions(string group)
        {
            try
            {
                HashSet<Permission> groupPersmissions;
                if (GroupPermissionsMapping.TryGetValue(group, out groupPersmissions))
                {
                    return groupPersmissions.ToList();
                }
                else
                {
                    return new List<Permission>();
                }
            }
            catch(Exception e)
            {
                Console.WriteLine("IZ RBACA"+e.Message.ToString());
            }

            return null;
           
        }

        static RBACService()
        {
            GroupPrincipal groupPrincipal;
            configuration = ParseConfiguration();

            foreach (var config in configuration)
            {
                using (groupPrincipal = GroupPrincipal.FindByIdentity(context, IdentityType.Name, config.Key))
                {
                    GroupPermissionsMapping[groupPrincipal.Sid.Value] = new HashSet<Permission>(config.Value);
                }
            }
        }

        private static Dictionary<string, List<Permission>> ParseConfiguration()
        {
            List<Group> groups;
            XmlSerializer serializer = new XmlSerializer(typeof(List<Group>));
            //ccccchangeee
            using (FileStream fs = File.Open(@"C:\Users\HP\Desktop\Projakat\CLS\CentralizedLoggingService\DBM\UserAccessConfig.xml", FileMode.OpenOrCreate))
            {
                groups = serializer.Deserialize(fs) as List<Group>;
            }

            Dictionary<string, List<Permission>> retval = new Dictionary<string, List<Permission>>(groups.Count);

            for (int i = 0; i < groups.Count; i++)
            {
                retval[groups[i].name] = groups[i].Permissions;
            }

            return retval;
        }

        private static void StoreConfiguration(Dictionary<string, List<Permission>> groups)
        {
            List<Group> groupsList = new List<Group>();
            XmlSerializer serializer = new XmlSerializer(typeof(List<Group>));

            foreach (var group in groups)
            {
                groupsList.Add(new Group(group.Key) { Permissions = group.Value });
            }
         
            using (FileStream fs = File.Open(@"C:\Users\HP\Desktop\Projakat\CLS\CentralizedLoggingService\DBM\UserAccessConfig.xml", FileMode.OpenOrCreate))
            {
                serializer.Serialize(fs, groupsList);
            }
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
