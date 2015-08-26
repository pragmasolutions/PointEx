using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Web.Mvc;
using Framework.Common.Web.Alerts;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using PagedList;
using PointEx.Data;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security;
using PointEx.Security.Managers;
using PointEx.Security.Model;
using PointEx.Service;
using PointEx.Web.Configuration;
using PointEx.Web.Controllers;
using PointEx.Web.Models;
using PointEx.Web.Infrastructure.Extensions;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [AuthorizeRoles(RolesNames.SuperAdmin, RolesNames.Admin)]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly ApplicationUserManager _userManager;

        public UserController(IUserService userService, ApplicationUserManager userManager)
        {
            _userService = userService;
            _userManager = userManager;
        }

        public ActionResult Index(UserListFiltersModel filters)
        {
            int pageTotal;
            
            var users = _userService.GetAllAdministrators(this.User, "UserName", "ASC", filters.Criteria, filters.Page, DefaultPageSize, out pageTotal);

            var pagedList = new StaticPagedList<UserDto>(users, filters.Page, DefaultPageSize, pageTotal);

            var listModel = new UserListModel(pagedList, filters);

            return View(listModel);
        }

        public ActionResult Detail(string id)
        {
            var user = _userService.GetById(id);
            var appUser = _userManager.FindById(id);

            return View(UserForm.Create(user, appUser));
        }

        public ActionResult Create()
        {
            var userForm = new UserForm();

            return View(userForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserForm userForm)
        {
            if (!ModelState.IsValid)
            {
                return View(userForm);
            }

            var user = new ApplicationUser { UserName = userForm.Email, Email = userForm.Email };

            try
            {
                var newRole = MapRoleFromEnum(userForm.RoleId);

                await _userService.Create(user, newRole.Name, AppSettings.Theme);
            }
            catch (ApplicationException ex)
            {
                this.ModelState.AddModelError("", ex.Message);
                return View(userForm);
            }

            return RedirectToAction("Index", new UserListFiltersModel().GetRouteValues()).WithSuccess("Usuario Creado");
        }

        public ActionResult Edit(string id)
        {
            var user = _userService.GetById(id);
            var appUser = _userManager.FindById(id);
            var userForm = UserForm.Create(user, appUser);

            return View(userForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Edit(string id, UserForm userForm)
        {
            if (!ModelState.IsValid)
            {
                return View(userForm);
            }
            var user = _userService.GetById(id);
            user.Email = userForm.Email;
            user.UserName = userForm.UserName;
            var newRole = MapRoleFromEnum(userForm.RoleId);
            user.Roles.Clear();
            user.Roles.Add(newRole);
            _userService.Edit(user);

            return RedirectToAction("Index", new UserListFiltersModel().GetRouteValues()).WithSuccess("Usuario Editado");
        }

        private Role MapRoleFromEnum(UserForm.AdminRoleEnum roleEnum)
        {
            var roleId = string.Empty;
            switch (roleEnum)
            {
                case UserForm.AdminRoleEnum.Admin:
                    roleId = Role.Admin;
                    break;
                case UserForm.AdminRoleEnum.ShopAdmin:
                    roleId = Role.ShopAdmin;
                    break;
                case UserForm.AdminRoleEnum.BeneficiaryAdmin:
                    roleId = Role.BeneficiaryAdmin;
                    break;
            }
            return _userService.GetRoleById(roleId);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Delete(string id, FormCollection collection)
        {
            _userService.Delete(id);

            return RedirectToAction("Index", new UserListFiltersModel().GetRouteValues()).WithSuccess("Usuario Eliminado");
        }
    }
}
