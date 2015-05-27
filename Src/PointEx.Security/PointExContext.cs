using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.ModelBinding;
using Framework.Data.EntityFramework.Helpers;
using PointEx.Data;
using PointEx.Entities;

namespace PointEx.Security
{
    public static class PointExContext // : IPointExContext
    {
        private static PointExUow _uow = new PointExUow(new RepositoryProvider(new RepositoryFactories())); 

        public static User User
        {
            get
            {
                if (HttpContext.Current.Session["User"] != null)
                    return HttpContext.Current.Session["User"] as User;
                if (HttpContext.Current.User.Identity != null)
                {
                    var user = _uow.Users.Get(u => u.UserName == HttpContext.Current.User.Identity.Name, u => u.Roles);
                    HttpContext.Current.Session["User"] = user;
                    return user;
                }
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

        public static Beneficiary Beneficiary
        {
            get
            {
                if (Role == RolesNames.Beneficiary)
                {
                    if (HttpContext.Current.Session["Beneficiary"] == null)
                    {

                        var beneficiary = _uow.Beneficiaries.Get(b => b.UserId == User.Id, b => b.User, b => b.Town,
                                                                                b => b.Cards, 
                                                                                b => b.Cards.Select(c => c.Purchases),
                                                                                b => b.EducationalInstitution,
                                                                                b => b.PointsExchanges);
                        HttpContext.Current.Session["Beneficiary"] = beneficiary;
                    }
                    return HttpContext.Current.Session["Beneficiary"] as Beneficiary;
                }
                return null;
            }
        }

        public static Shop Shop
        {
            get
            {
                if (Role == RolesNames.Shop)
                {
                    if (HttpContext.Current.Session["Shop"] == null)
                    {

                        var shop = _uow.Shops.Get(s => s.UserId == User.Id, s => s.User, s => s.Benefits,
                                                                                s => s.Purchases, s => s.ShopCategories,
                                                                                s => s.Town);
                        HttpContext.Current.Session["Shop"] = shop;
                    }
                    return HttpContext.Current.Session["Shop"] as Shop;
                }
                return null;
            }
        }

        public static void SetIdentity(User user)
        {
            System.Web.HttpContext.Current.Session["User"] = user;
        }
    }
}
