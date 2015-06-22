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
using PointEx.Security.Model;
using Resources;

namespace PointEx.Web.Models
{
    public class UserForm : IMapFrom<User>
    {
        [HiddenInput]
        public string Id { get; set; }

        [Required]
        [Display(Name = "Nombre de usuario")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress(ErrorMessageResourceType = typeof(PointExGlobalResources), ErrorMessageResourceName = "EmailAddress", ErrorMessage = null)]
        [Display(Name = "Email")]
        public string Email { get; set; }

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

            return form;
        }
    }
}
