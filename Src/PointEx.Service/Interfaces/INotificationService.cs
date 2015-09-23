using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Entities.Models;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public interface INotificationService
    {
        Task SendPointsExchangeConfirmationEmail(Prize prize, Beneficiary beneficiary, string[] emailsAdmin, DateTime exchangeDate, string theme);
        Task SendAccountConfirmationEmail(string userId, string theme);
        Task SendInformationRequestEmail(InformationRequestModel request, string theme);
        Task SendPendingBenefitEmail(string benefitName, string beneficiaryEmail, bool created, string theme);
        Task SendAddShopRequestEmail(Shop shop, string email, string theme);
        Task SendAddBeneficiaryRequestEmail(Beneficiary beneficiary, string email, string theme);
        Task SendBenefitApprovedMail(Benefit benefit, string siteBaseUrl);
    }
}