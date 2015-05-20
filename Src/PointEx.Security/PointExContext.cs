using System;
using System.Linq;
using System.Threading;
using PointEx.Entities;

namespace PointEx.Security
{
    public class PointExContext : IPointExContext
    {
        public User User
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["User"] != null)
                    return System.Web.HttpContext.Current.Session["User"] as User;
                return null;
            }
        }

        public bool IsInRole(string role)
        {
            return Role == role;
        }
        
        public string Role
        {
            get
            {
                if (User != null && User.Roles != null)
                {
                    return User.Roles.FirstOrDefault().Name;
                }
                return null;
            }
        }

        private Beneficiary _beneficiary;
        public Beneficiary Beneficiary
        {
            get
            {
                if (_beneficiary != null)
                    return _beneficiary;
                if (Role == RolesNames.Beneficiario)
                {
                    
                    //TODO:
                    //var repo = new Framework.Data.EntityFramework.Repository.EFRepository<Beneficiary>();
                }
                return null;
            }
        }
    }

    
}
