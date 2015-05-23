using System.Security.Principal;
using Microsoft.AspNet.Identity;
using PointEx.Entities;
using PointEx.Security.Model;
using PointEx.Service;

namespace PointEx.Web.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity _identity;
        private readonly IShopService _shopService;

        private Shop _shop;

        public CurrentUser(IIdentity identity, IShopService shopService)
        {
            _identity = identity;
            _shopService = shopService;
        }

        public Shop Shop
        {
            get
            {
                return _shop ?? (_shop = _shopService.GetByUserId(_identity.GetUserId()));
            }
        }
    }
}