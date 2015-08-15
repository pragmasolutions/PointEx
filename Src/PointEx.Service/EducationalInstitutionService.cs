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
    public class EducationalInstitutionService : ServiceBase, IEducationalInstitutionService
    {
        private readonly IClock _clock;

        public EducationalInstitutionService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public IQueryable<EducationalInstitution> GetAll()
        {
            return Uow.EducationalInstitutions.GetAll().Where(e => !e.IsDeleted);
        }

        public List<EducationalInstitutionDto> GetAll(string sortBy, string sortDirection, string criteria, int? townId, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<EducationalInstitution, bool>> where = x => !x.IsDeleted && 
                                                        ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                        (!townId.HasValue || x.TownId == townId));

            var results = Uow.EducationalInstitutions.GetAll(pagingCriteria,
                                                    where,
                                                    x => x.Town);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<EducationalInstitutionDto>().ToList();
        }

        public EducationalInstitution GetById(int id)
        {
            return Uow.EducationalInstitutions.Get(id);
        }

        public EducationalInstitution GetByName(string name)
        {
            return Uow.EducationalInstitutions.Get(e => e.Name == name && !e.IsDeleted);
        }

        public void Create(EducationalInstitution educationalInstitution)
        {
            if (!IsNameAvailable(educationalInstitution.Name, educationalInstitution.Id))
            {
                throw new ApplicationException("Un Establecimiento Educativo con el mismo nombre ya ha sido creado");
            }

            educationalInstitution.CreatedDate = _clock.Now;
            Uow.EducationalInstitutions.Add(educationalInstitution);
            Uow.Commit();
        }

        public void Edit(EducationalInstitution educationalInstitution)
        {
            var currentEducationalInstitution = this.GetById(educationalInstitution.Id);

            currentEducationalInstitution.ModifiedDate = _clock.Now;
            currentEducationalInstitution.Name = educationalInstitution.Name;
            currentEducationalInstitution.TownId = educationalInstitution.TownId;
            currentEducationalInstitution.Location = educationalInstitution.Location;

            Uow.EducationalInstitutions.Edit(currentEducationalInstitution);
            Uow.Commit();
        }

        public void Delete(int educationalInstitutionId)
        {
            var beneficiaries = Uow.Beneficiaries.GetAll()
                    .Where(b => b.EducationalInstitutionId == educationalInstitutionId && !b.IsDeleted)
                    .ToList();

            foreach (var beneficiary in beneficiaries)
            {
                beneficiary.IsDeleted = true;
                Uow.Beneficiaries.Edit(beneficiary);
            }

            if (beneficiaries.Any())
            {
                var school = Uow.EducationalInstitutions.Get(educationalInstitutionId);
                school.IsDeleted = true;
                Uow.EducationalInstitutions.Edit(school);
            }
            else
            {
                Uow.EducationalInstitutions.Delete(educationalInstitutionId);
            }
            Uow.Commit();
        }

        public bool IsNameAvailable(string name, int id)
        {
            var currentEducationalInstitution = this.GetByName(name);

            if (currentEducationalInstitution == null)
            {
                return true;
            }

            return currentEducationalInstitution.Id == id;
        }

        public bool CanRemove(int educationInstitutionId)
        {
            var beneficiaries = Uow.Beneficiaries.GetAll().Where(b => educationInstitutionId == b.EducationalInstitutionId && !b.IsDeleted);

            if (beneficiaries.Any())
            {
                return false;
            }
            
            return true;
        }


        public List<EducationalInstitution> GetByTown(int townId)
        {
            return Uow.EducationalInstitutions.GetAll().Where(e => e.TownId == townId).ToList();
        }
    }
}
