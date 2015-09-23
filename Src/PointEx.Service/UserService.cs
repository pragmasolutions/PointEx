using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Compilation;
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
    public class UserService : ServiceBase, IUserService
    {
        private readonly ApplicationUserManager _userManager;
        private readonly INotificationService _notificationService;
        private readonly IClock _clock;

        public UserService(IPointExUow uow, ApplicationUserManager userManager, INotificationService notificationService, IClock clock)
        {
            _userManager = userManager;
            _notificationService = notificationService;
            _clock = clock;
            Uow = uow;
        }

        public async Task Create(ApplicationUser applicationUser, string roleName, string theme)
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

                    await _userManager.AddToRoleAsync(applicationUser.Id, roleName);

                    try
                    {
                        await _notificationService.SendAccountConfirmationEmail(applicationUser.Id, theme);
                    }
                    catch (Exception)
                    {
                        
                    }
                    trasactionScope.Complete();
                }
                catch (Exception ex)
                {
                    trasactionScope.Dispose();
                    throw ex;
                }
            }
        }

        public void Edit(User user)
        {
            Uow.Users.Edit(user);
            Uow.Commit();
        }

        public void Delete(string userId)
        {
            var user = Uow.Users.Get(u => u.Id == userId);

            Uow.Users.Delete(user);

            Uow.Commit();
        }

        public IQueryable<User> GetAll()
        {
            return Uow.Users.GetAll(whereClause: null, includes: u => u.Roles);
        }

        public User GetById(string id)
        {
            return Uow.Users.Get(u => u.Id == id, s => s.Roles);
        }
        
        public Role GetRoleById(string roleId)
        {
            return Uow.Roles.Get(r => r.Id == roleId);
        }

        public List<User> GetUsersBeneficiaryAdmin()
        {
            return Uow.Users.GetAll().Where(u => u.Roles.Any(r => r.Name == RolesNames.BeneficiaryAdmin)).ToList();
            
        }

        public List<UserDto> GetAll(string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "UserName",
                SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "ASC"
            };


            Expression<Func<User, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.UserName.Contains(criteria)) &&
                                                           (string.IsNullOrEmpty(criteria) || x.Email.Contains(criteria)));

            var results = Uow.Users.GetAll(pagingCriteria,
                                                    where,
                                                     s => s.Roles);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<UserDto>().ToList();
        }

        public List<UserDto> GetAllAdministrators(IPrincipal currentUser,  string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria
            {
                PageNumber = pageIndex,
                PageSize = pageSize,
                SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "UserName",
                SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "ASC"
            };

            Expression<Func<User, bool>> where;
            if (currentUser.IsInRole(RolesNames.SuperAdmin))
            {
                where = x => ((string.IsNullOrEmpty(criteria) || x.UserName.Contains(criteria)) &&
                                                           (string.IsNullOrEmpty(criteria) || x.Email.Contains(criteria)) &&
                                                           (x.Roles.Any(r => r.Name == RolesNames.Admin)
                                                           || x.Roles.Any(r => r.Name == RolesNames.BeneficiaryAdmin)
                                                           || x.Roles.Any(r => r.Name == RolesNames.ShopAdmin)));
            }
            else
            {
                where = x => ((string.IsNullOrEmpty(criteria) || x.UserName.Contains(criteria)) &&
                                                           (string.IsNullOrEmpty(criteria) || x.Email.Contains(criteria)) &&
                                                           (x.Roles.Any(r => r.Name == RolesNames.BeneficiaryAdmin)
                                                           || x.Roles.Any(r => r.Name == RolesNames.ShopAdmin)));
            }

            var results = Uow.Users.GetAll(pagingCriteria,
                                                    where,
                                                     s => s.Roles);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<UserDto>().ToList();
        }
    }
}
