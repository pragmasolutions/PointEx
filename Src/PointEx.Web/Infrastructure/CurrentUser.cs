using System.Security.Principal;
using Microsoft.AspNet.Identity;
using PointEx.Entities;
using PointEx.Security.Managers;
using PointEx.Security.Model;
using PointEx.Service;

namespace PointEx.Web.Infrastructure
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IIdentity _identity;
        private readonly IShopService _shopService;
        private readonly IUserService _userService;
        private readonly ApplicationUserManager _applicationUserManager;
        private readonly IBeneficiaryService _beneficiaryService;

        private ApplicationUser _user;
        private Shop _shop;
        private Beneficiary _beneficiary;

        public CurrentUser(IIdentity identity, IShopService shopService, ApplicationUserManager applicationUserManager, IBeneficiaryService beneficiaryService, IUserService userService)
        {
            _identity = identity;
            _shopService = shopService;
            _userService = userService;
            _applicationUserManager = applicationUserManager;
            _beneficiaryService = beneficiaryService;
        }
        
        public Shop Shop
        {
            get
            {
                return _shop ?? (_shop = _shopService.GetByUserId(_identity.GetUserId()));
            }
        }

        public Beneficiary Beneficiary
        {
            get
            {
                return _beneficiary ?? (_beneficiary = _beneficiaryService.GetByUserId(_identity.GetUserId()));
            }
        }

        public ApplicationUser User
        {
            get { return _user ?? (_user = _applicationUserManager.FindById(_identity.GetUserId())); }
        }
    }
}