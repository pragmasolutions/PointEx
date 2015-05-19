using System.Linq;
using System.Threading;
using PointEx.Entities;

namespace PointEx.Security
{
    public class PointExContext : IPointExContext
    {
        public User User
        {
            get { return PointExIdentity.User; }
        }

        private PointExIdentity PointExIdentity
        {
            get
            {
                var PointExPrincipal = Thread.CurrentPrincipal as PointExPrincipal;
                return PointExPrincipal != null ? PointExPrincipal.Identity as PointExIdentity : null;
            }
        }

        private PointExPrincipal PointExPrincipal
        {
            get
            {
                return Thread.CurrentPrincipal as PointExPrincipal;
            }
        }

        public bool IsInRole(string role)
        {
            return PointExPrincipal.IsInRole(role);
        }
        
        Role IPointExContext.Role
        {
            get { return User.Roles.FirstOrDefault(); }
        }
    }
}
