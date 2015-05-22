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
    public class PrizeService : ServiceBase, IPrizeService
    {
        private readonly IClock _clock;

        public PrizeService(IPointExUow uow, IClock clock)
        {
            _clock = clock;
            Uow = uow;
        }

        public IQueryable<Prize> GetAll()
        {
            return Uow.Prizes.GetAll();
        }

        public List<PrizeDto> GetAll(string sortBy, string sortDirection, string criteria, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Prize, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)));

            var results = Uow.Prizes.GetAll(pagingCriteria,
                                                    where);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<PrizeDto>().ToList();
        }

        public Prize GetById(int id)
        {
            return Uow.Prizes.Get(id);
        }

        public Prize GetByName(string name)
        {
            return Uow.Prizes.Get(e => e.Name == name);
        }

        public void Create(Prize prize)
        {
            if (!IsNameAvailable(prize.Name, prize.Id))
            {
                throw new ApplicationException("Un Premio con el mismo nombre ya ha sido creado");
            }

            prize.CreatedDate = _clock.Now;
            Uow.Prizes.Add(prize);
            Uow.Commit();
        }

        public void Edit(Prize prize)
        {
            var currentPrize = this.GetById(prize.Id);

            currentPrize.ModifiedDate = _clock.Now;
            currentPrize.Name = prize.Name;
            currentPrize.PointsNeeded = prize.PointsNeeded;
            currentPrize.Description = prize.Description;

            Uow.Prizes.Edit(currentPrize);
            Uow.Commit();
        }

        public void Delete(int prizeId)
        {
            Uow.Prizes.Delete(prizeId);
            Uow.Commit();
        }

        public bool IsNameAvailable(string name, int id)
        {
            var currentPrize = this.GetByName(name);

            if (currentPrize == null)
            {
                return true;
            }

            return currentPrize.Id == id;
        }
    }
}
