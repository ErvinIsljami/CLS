using DBM.RBAC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    [Serializable]
    public class Group
    {
        public Group()
        {
        }
        public Group(string name)
        {
            this.name = name;
        }
        public string name { get; set; }
        public List<Permission> Permissions { get; set; }
    }
}
