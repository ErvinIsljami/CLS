using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class DatabaseException
    {
   
        private string _reason;

        [DataMember]
        public string Reason
        {
            get { return _reason; }
            set { _reason = value; }
        }
    }
}
