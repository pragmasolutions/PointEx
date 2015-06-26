using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Service.Exceptions;
using PointEx.Entities.Enums;

namespace PointEx.Service
{
    public class BenefitTypesService : ServiceBase, IBenefitTypesService
    {
        private readonly IClock _clock;        
        
        public BenefitTypesService(IPointExUow uow, IClock clock)
        {
            _clock = clock;            
            Uow = uow;
        }

        public IQueryable<BenefitType> GetAll()
        {
            return Uow.BenefitTypes.GetAll();
        }

        public BenefitType GetById(int id)
        {
            return Uow.BenefitTypes.Get(b => b.Id == (BenefitTypesEnum)id);
        }
    }
}
