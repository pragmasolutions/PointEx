﻿using System;
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
    public class BenefitService : ServiceBase, IBenefitService
    {
        private readonly IClock _clock;

        public BenefitService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public IQueryable<Benefit> GetAll()
        {
            return Uow.Benefits.GetAll();
        }

        public List<BenefitDto> GetAll(string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Benefit, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Description.Contains(criteria)));

            var results = Uow.Benefits.GetAll(pagingCriteria, where);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BenefitDto>().ToList();
        }

        public Benefit GetById(int id)
        {
            return Uow.Benefits.Get(id);
        }

        public Benefit GetByName(string name)
        {
            return Uow.Benefits.Get(e => e.Description == name);
        }

        public void Create(Benefit benefit)
        {
            if (!IsNameAvailable(benefit.Description, benefit.Id))
            {
                throw new ApplicationException("Un Beneficio con el mismo nombre ya ha sido creado");
            }

            benefit.CreatedDate = _clock.Now;
            Uow.Benefits.Add(benefit);
            Uow.Commit();
        }

        public void Edit(Benefit benefit)
        {
            var currentBenefit = this.GetById(benefit.Id);

            currentBenefit.ModifiedDate = _clock.Now;
            currentBenefit.Description = benefit.Description;

            Uow.Benefits.Edit(currentBenefit);
            Uow.Commit();
        }

        public void Delete(int benefitId)
        {
            Uow.Benefits.Delete(benefitId);
            Uow.Commit();
        }

        public bool IsNameAvailable(string name, int id)
        {
            var currentBenefit = this.GetByName(name);

            if (currentBenefit == null)
            {
                return true;
            }

            return currentBenefit.Id == id;
        }
    }
}