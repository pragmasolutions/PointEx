using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class PointsExchangeService : ServiceBase, IPointsExchangeService
    {
        private readonly IClock _clock;
        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IPrizeService _prizeService;
        private readonly INotificationService _notificationService;
        private readonly IUserService _userService;

        public PointsExchangeService(IPointExUow uow, IClock clock, 
            IBeneficiaryService beneficiaryService, 
            IPrizeService prizeService, 
            INotificationService notificationService,
            IUserService userService)
        {
            _clock = clock;
            _beneficiaryService = beneficiaryService;
            _prizeService = prizeService;
            _notificationService = notificationService;
            _userService = userService;

            Uow = uow;
        }

        public async Task ExchangePoints(int prizeId, int beneficiaryId, string theme)
        {
            using (var trasactionScope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            {
                try
                {
                    var beneficiary = _beneficiaryService.GetById(beneficiaryId);

                    var prize = _prizeService.GetById(prizeId);

                    if (prize.PointsNeeded > beneficiary.Points)
                    {
                        throw new ApplicationException("Puntos insuficientes");
                    }

                    PointsExchange pointsExchange = new PointsExchange();

                    pointsExchange.BeneficiaryId = beneficiary.Id;
                    pointsExchange.PrizeId = prize.Id;
                    pointsExchange.PointsUsed = prize.PointsNeeded;
                    pointsExchange.ExchangeDate = _clock.Now;

                    Uow.PointsExchanges.Add(pointsExchange);

                    var users = _userService.GetUsersBeneficiaryAdmin();
                    string[] emailsAdmin = users.Select(u => u.Email).ToArray();
                    //users.RemoveAt(0);
                    //foreach (var userDto in users)
                    //{
                    //    emailsAdmin = string.Format("{0};{1}",emailsAdmin, userDto.Email);
                    //}

                    await _notificationService.SendPointsExchangeConfirmationEmail(prize, beneficiary, emailsAdmin, pointsExchange.ExchangeDate, theme);

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
    }
}
