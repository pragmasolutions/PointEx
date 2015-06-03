using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class BenefitBranchOfficeService : ServiceBase, IBenefitBranchOfficeService
    {
        private readonly IClock _clock;

        public BenefitBranchOfficeService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public BenefitBranchOffice GetById(int id)
        {
            return Uow.BenefitBranchOffice.Get(id);            
        }       

        public void Create(BenefitBranchOffice benefitBranchOffice)
        {            
            Uow.BenefitBranchOffice.Add(benefitBranchOffice);
            Uow.Commit();
        }

        public void Delete(int benefitBranchOfficeId)
        {
            Uow.Benefits.Delete(benefitBranchOfficeId);
            Uow.Commit();
        }      
    }
}
