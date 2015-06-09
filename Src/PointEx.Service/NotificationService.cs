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
using PointEx.Security.Managers;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public class NotificationService : INotificationService
    {
        private readonly ApplicationUserManager _userManager;

        public NotificationService(ApplicationUserManager userManager)
        {
            _userManager = userManager;
        }

        public async Task SendAccountConfirmationEmail(string userId)
        {
            var urlhelper = new UrlHelper(HttpContext.Current.Request.RequestContext);
            string code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
            var callbackUrl = urlhelper.Action("FirstLogin", "Account", new { area = "", userId = userId, code = code }, protocol: HttpContext.Current.Request.Url.Scheme);
            await _userManager.SendEmailAsync(userId, "Confirme su Cuenta", "Por favor confirme su cuenta y cambie su contraseña haciendo click <a href=\"" + callbackUrl + "\">aquí</a>");
        }
    }
}
