using System.Linq;
using System.Security.Principal;

namespace PointEx.Security
{
    public class PointExPrincipal : IPrincipal
    {
        private PointExIdentity _identity;

        public PointExIdentity Identity
        {
            get { return _identity ?? new AnonymousIdentity(); }
            set { _identity = value; }
        }

        #region IPrincipal Members
        IIdentity IPrincipal.Identity
        {
            get { return this.Identity; }
        }

        public bool IsInRole(string role)
        {
            return _identity.Roles.Contains(role);
        }
        #endregion
    }
    
}
