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
        Task SendPointsExchangeConfirmationEmail(Prize prize, Beneficiary beneficiary, DateTime exchangeDate, string theme);
        Task SendAccountConfirmationEmail(string userId);
        Task SendInformationRequestEmail(InformationRequestModel request, string theme);
    }
}