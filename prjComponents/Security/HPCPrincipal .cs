using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Principal;

namespace prjComponents
{
    public class HPCPrincipal : GenericPrincipal
    {
        public HPCPrincipal(IIdentity identity, string[] roles): base(identity, roles)
        {
        }
        public new IdentityUser Identity
        {
            get
            {
                if (base.Identity is IdentityUser)
                {
                    return base.Identity as IdentityUser;
                }

                IdentityUser r = new IdentityUser(base.Identity.Name, "prjComponents");
                return r;
            }
        }
    }
}
