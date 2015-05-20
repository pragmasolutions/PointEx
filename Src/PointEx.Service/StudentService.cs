using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper.QueryableExtensions;
using Framework.Common.Utility;
using Framework.Data.Helpers;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public class BeneficiaryService : ServiceBase, IBeneficiaryService
    {
        private readonly IClock _clock;

        public BeneficiaryService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public void Create(Beneficiary student)
        {
            student.CreatedDate = _clock.Now;
            Uow.Beneficiaries.Add(student);
            Uow.Commit();
        }

        public void Edit(Beneficiary student)
        {
            student.ModifiedDate = _clock.Now;
            Uow.Beneficiaries.Edit(student);
            Uow.Commit();
        }

        public void Delete(int shopId)
        {
            var student = this.GetById(shopId);
            Uow.Beneficiaries.Delete(student);
            Uow.Commit();
        }

        public IQueryable<Beneficiary> GetAll()
        {
            return Uow.Beneficiaries.GetAll(whereClause: null, includes: s => s.Town);
        }

        public Beneficiary GetById(int id)
        {
            return Uow.Beneficiaries.Get(s => s.Id == id, s => s.Town, s => s.EducationalInstitution);
        }

        public List<BeneficiaryDto> GetAll(string sortBy, string sortDirection, string criteria, int? category, int? townId, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Beneficiary, bool>> where = x => (
                                                         //((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                         (!townId.HasValue || x.TownId == townId));

            var results = Uow.Beneficiaries.GetAll(pagingCriteria,
                                                    where,
                                                     s => s.Town, s => s.EducationalInstitution);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<BeneficiaryDto>().ToList();
        }
    }
}
