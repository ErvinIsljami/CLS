using System;
using System.Collections.Generic;
using System.IdentityModel.Claims;
using System.IdentityModel.Policy;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace DBM
{
    public class MyAuthorizationPolicy : IAuthorizationPolicy
    {
        private string id;
        public string Id { get; private set; }

        public MyAuthorizationPolicy()
        {
            Id = Guid.NewGuid().ToString();
        }

        public bool Evaluate(EvaluationContext evaluationContext, ref object state)
        {
            foreach (var prop in evaluationContext.Properties)
            {
                Console.WriteLine(prop.Key);
            }

            IIdentity identity = (evaluationContext.Properties["Identities"] as List<IIdentity>)?[0];
           // Services.identity = identity;
            evaluationContext.Properties.Add("Principal", new MyPrincipal(identity));
            return true;
        }

        public ClaimSet Issuer { get; }
    }
}
