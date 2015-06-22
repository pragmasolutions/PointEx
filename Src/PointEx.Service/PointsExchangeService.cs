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

        public PointsExchangeService(IPointExUow uow, IClock clock, IBeneficiaryService beneficiaryService, IPrizeService prizeService, INotificationService notificationService)
        {
            _clock = clock;
            _beneficiaryService = beneficiaryService;
            _prizeService = prizeService;
            _notificationService = notificationService;
            Uow = uow;
        }

        public async Task ExchangePoints(int prizeId, int beneficiaryId)
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

                    await _notificationService.SendPointsExchangeConfirmationEmail(prize, beneficiary, pointsExchange.ExchangeDate);

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

        public IList<PointsExchange> GetAllByBeneficiaryId(int beneficiaryId)
        {
            return Uow.PointsExchanges.GetAll(pe => pe.BeneficiaryId == beneficiaryId).ToList();
        }
    }
}
