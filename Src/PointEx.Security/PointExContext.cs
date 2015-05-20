using System;
using System.Linq;
using System.Threading;
using System.Web.ModelBinding;
using PointEx.Entities;

namespace PointEx.Security
{
    public static class PointExContext // : IPointExContext
    {
        public static User User
        {
            get
            {
                if (System.Web.HttpContext.Current.Session["User"] != null)
                    return System.Web.HttpContext.Current.Session["User"] as User;
                return null;
            }
        }

        public static bool IsInRole(string role)
        {
            return Role == role;
        }

        public static string Role
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

        private static Beneficiary _beneficiary;
        public static Beneficiary Beneficiary
        {
            get
            {
                if (_beneficiary != null)
                    return _beneficiary;
                if (Role == RolesNames.Beneficiario)
                {
                    _beneficiary = System.Web.HttpContext.Current.Session["Beneficiary"] as Beneficiary;
                    return _beneficiary;
                }
                return null;
            }
        }

        public static void SetIdentity(User user)
        {
            System.Web.HttpContext.Current.Session["User"] = user;
        }

        public static void SetIdentity(User user, Beneficiary beneficiary)
        {
            SetIdentity(user);
            System.Web.HttpContext.Current.Session["Beneficiary"] = beneficiary;
        }

        public static void SetIdentity(User user, Shop shop)
        {
            SetIdentity(user);
            System.Web.HttpContext.Current.Session["Shop"] = shop;
        }
    }
}
