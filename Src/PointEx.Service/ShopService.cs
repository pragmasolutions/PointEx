using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using Microsoft.AspNet.Identity;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Security.Managers;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public class ShopService : ServiceBase, IShopService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly INotificationService _notificationService;
        private readonly IClock _clock;

        public ShopService(IPointExUow uow, ApplicationUserManager userManager, INotificationService notificationService, IClock clock)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _clock = clock;
            Uow = uow;
        }

        public async Task Create(Shop shop, ApplicationUser applicationUser)
        {
            using (var trasactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (_userManager.FindByEmail(applicationUser.Email) != null)
                    {
                        throw new ApplicationException("Ya existe un usuario con ese email.");
                    }

                    var result = await _userManager.CreateAsync(applicationUser);

                    if (!result.Succeeded)
                    {
                        throw new ApplicationException(result.Errors.FirstOrDefault());
                    }

                    await _userManager.AddToRoleAsync(applicationUser.Id, RolesNames.Shop);

                    shop.CreatedDate = _clock.Now;
                    shop.UserId = applicationUser.Id;
                    Uow.Shops.Add(shop);

                    await Uow.CommitAsync();

                    await _notificationService.SendAccountConfirmationEmail(applicationUser.Id);

                    trasactionScope.Complete();
                }
                catch (Exception ex)
                {
                    trasactionScope.Dispose();
                    throw ex;
                }
            }
        }

        public void Edit(Shop shop)
        {
            var currentShop = this.GetById(shop.Id);

            foreach (var category in currentShop.ShopCategories.ToArray())
            {
                Uow.ShopCategories.Delete(category);
            }

            foreach (var category in shop.ShopCategories)
            {
                Uow.ShopCategories.Add(category);
            }

            currentShop.Name = shop.Name;
            currentShop.Address = shop.Address;
            currentShop.Phone = shop.Phone;
            currentShop.TownId = shop.TownId;
            currentShop.Location = shop.Location;
            currentShop.ModifiedDate = _clock.Now;
            currentShop.User.Email = shop.User.Email;

            Uow.Shops.Edit(currentShop);
            Uow.Commit();
        }

        public void Delete(int shopId)
        {
            var shop = this.GetById(shopId);

            foreach (var category in shop.ShopCategories.ToArray())
            {
                Uow.ShopCategories.Delete(category);
                shop.ShopCategories.Remove(category);
            }

            Uow.Shops.Delete(shop);
            Uow.Commit();
        }

        public IQueryable<Shop> GetAll()
        {
            return Uow.Shops.GetAll(whereClause: s => true, includes: s => s.Town);
        }

        public Shop GetById(int id)
        {
            return Uow.Shops.Get(s => s.Id == id, s => s.ShopCategories.Select(cs => cs.Category),s => s.User);
        }

        public Shop GetByUserId(string userId)
        {
            return Uow.Shops.Get(s => s.UserId == userId);
        }

        public List<ShopDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Shop, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                      (!category.HasValue || x.ShopCategories.Any(c => c.Id == category)) &&
                                                      (!townId.HasValue || x.TownId == townId));

            var results = Uow.Shops.GetAll(pagingCriteria,
                                                    where,
                                                    x => x.ShopCategories.Select(sc => sc.Category),
                                                    x => x.Town);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<ShopDto>().ToList();
        }
    }
}
