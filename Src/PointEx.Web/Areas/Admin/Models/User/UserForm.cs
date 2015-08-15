using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Framework.Common.Mapping;
using PointEx.Entities;
using PointEx.Security;
using PointEx.Security.Model;
using Resources;

namespace PointEx.Web.Models
{
    
    public class UserForm : IMapFrom<User>
    {
        public enum AdminRoleEnum
        {
            Admin = 1,
            BeneficiaryAdmin = 2,
            ShopAdmin = 3
        }

        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(PointExGlobalResources), ErrorMessageResourceName = "EmailAddress", ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Rol")]
        [UIHint("RoleId")]
        public AdminRoleEnum RoleId { get; set; }

        public User ToUser()
        {
            var user = Mapper.Map<UserForm, User>(this);
            return user;
        }

        public static UserForm Create(User user, ApplicationUser appUser)
        {
            var form = Mapper.Map<User, UserForm>(user);
            form.UserName = user.UserName;
            form.Email = user.Email;

            switch (user.Roles.First().Name)
            {
                case RolesNames.Admin:
                    form.RoleId = AdminRoleEnum.Admin;
                    break;
                case RolesNames.ShopAdmin:
                    form.RoleId = AdminRoleEnum.ShopAdmin;
                    break;
                case RolesNames.BeneficiaryAdmin:
                    form.RoleId = AdminRoleEnum.BeneficiaryAdmin;
                    break;
            }
            return form;
        }
    }
}
