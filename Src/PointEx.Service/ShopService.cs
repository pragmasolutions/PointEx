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
using PointEx.Service.Exceptions;
using PointEx.Entities.Enums;
using System.Security.Principal;

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

        public async Task Create(Shop shop, string shopEmail, string theme)
        {
            using (var trasactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    if (_userManager.FindByEmail(shopEmail) != null)
                    {
                        throw new ApplicationException("Ya existe un usuario con ese email.");
                    }

                    var applicationUser = new ApplicationUser { UserName = shopEmail, Email = shopEmail };

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
            Edit(shop, null);
        }

        public void Edit(Shop shop, string shopEmail)
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

            if (!string.IsNullOrEmpty(shopEmail))
            {
                currentShop.User.Email = shopEmail;
            }

            if (shopEmail != null)
            {
                currentShop.StatusId = StatusEnum.Pending;
            }

            Uow.Shops.Edit(currentShop);

            Uow.Commit();
        }

        public void Delete(int shopId)
        {
            var shop = this.GetById(shopId);

            if (shop == null)
            {
                throw new NotFoundException("No encontro el comercio");
            }

            if (CanRemoveShop(shopId))
            {
                foreach (var category in shop.ShopCategories.ToArray())
                {
                    Uow.ShopCategories.Delete(category);
                    shop.ShopCategories.Remove(category);
                }

                foreach (var branchOffice in shop.BranchOffices.ToArray())
                {
                    Uow.BranchOffices.Delete(branchOffice);
                    shop.BranchOffices.Remove(branchOffice);
                }

                Uow.Users.Delete(shop.User);
                Uow.Shops.Delete(shop);
            }
            else
            {
                shop.IsDeleted = true;
                shop.User.IsDeleted = true;

                //TODO: mark benefits as deleted.
            }

            Uow.Commit();
        }

        public IQueryable<Shop> GetAll()
        {
            return Uow.Shops.GetAll(whereClause: s => !s.IsDeleted, includes: s => s.Town);
        }

        public Shop GetById(int id)
        {
            return Uow.Shops.Get(s => s.Id == id && !s.IsDeleted,
                s => s.ShopCategories.Select(cs => cs.Category),
                s => s.User,
                s => s.BranchOffices);
        }

        public Shop GetByUserId(string userId)
        {
            return Uow.Shops.Get(s => s.UserId == userId && !s.IsDeleted);
        }

        public List<ShopDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, bool? deleted, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Shop, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                      (!category.HasValue || x.ShopCategories.Any(c => c.Id == category)) &&
                                                      (!townId.HasValue || x.TownId == townId) &&
                                                      (!deleted.HasValue || x.IsDeleted == deleted));

            var results = Uow.Shops.GetAll(pagingCriteria,
                                                    where,
                                                    x => x.ShopCategories.Select(sc => sc.Category),
                                                    x => x.Town);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<ShopDto>().ToList();
        }

        private bool ShopHasPurchase(int shopId)
        {
            return Uow.Purchases.GetAll(pe => pe.ShopId == shopId).Any();
        }

        private bool ShopHasBenefits(int shopId)
        {
            return Uow.Benefits.GetAll(pe => pe.ShopId == shopId).Any();
        }

        private bool CanRemoveShop(int shopId)
        {
            if (ShopHasPurchase(shopId))
            {
                return false;
            }

            if (ShopHasBenefits(shopId))
            {
                return false;
            }

            return true;
        }

        public List<ShopDto> GetShopByStatus(string sortBy, string sortDirection, int? categoryId, int? townId, StatusEnum status, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Shop, bool>> where =
                x =>
                    ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria) || x.Name.Contains(criteria)) &&
                     (!categoryId.HasValue || x.ShopCategories.Any(c => c.CategoryId == categoryId)) &&
                     (!townId.HasValue || x.TownId == townId) &&
                     !x.IsDeleted && x.StatusId == status);

            var results = Uow.Shops.GetAll(pagingCriteria, where,
                //Includes
                b => b.ShopCategories);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<ShopDto>().ToList();
        }

        public void Moderated(int shopId, int statusId)
        {
            var currentShop = this.GetById(shopId);
            currentShop.StatusId = (StatusEnum)statusId;

            Uow.Shops.Edit(currentShop);

            Uow.Commit();
        }
    }
}
