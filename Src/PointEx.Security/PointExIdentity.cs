using System.Linq;
using System.Security.Principal;
using PointEx.Entities;

namespace PointEx.Security
{
    public class PointExIdentity : IIdentity
    {
        public PointExIdentity(User user)
        {
            Name = user.UserName;
            Email = string.Empty;
            Roles = user.Roles.Select(r => r.Name).ToArray();
        }

        public string Name { get; private set; }
        public string Email { get; private set; }
        public string[] Roles { get; private set; }
        public User User { get; private set; }

        #region IIdentity Members

        public string AuthenticationType
        {
            get { return "PointEx authentication"; }
        }

        public bool IsAuthenticated
        {
            get { return !string.IsNullOrEmpty(Name); }
        }

        #endregion
    }

    public class AnonymousIdentity : PointExIdentity
    {
        public AnonymousIdentity()
            : base(new User())
        { }
    }
}
