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

        public List<PrizeDto> GetAll(string sortBy, string sortDirection, string criteria, int? maxPointsNeeded, int pageIndex, int pageSize, out int pageTotal)
        {
            var pagingCriteria = new PagingCriteria();

            pagingCriteria.PageNumber = pageIndex;
            pagingCriteria.PageSize = pageSize;
            pagingCriteria.SortBy = !string.IsNullOrEmpty(sortBy) ? sortBy : "CreatedDate";
            pagingCriteria.SortDirection = !string.IsNullOrEmpty(sortDirection) ? sortDirection : "DESC";

            Expression<Func<Prize, bool>> where = x => ((string.IsNullOrEmpty(criteria) || x.Name.Contains(criteria)) &&
                                                        (!maxPointsNeeded.HasValue || x.PointsNeeded <= maxPointsNeeded));

            var results = Uow.Prizes.GetAll(pagingCriteria, where);

            pageTotal = results.PagedMetadata.TotalItemCount;

            return results.Entities.Project().To<PrizeDto>().ToList();
        }

        public Prize GetById(int id)
        {
            return Uow.Prizes.Get(p => p.Id == id, p => p.File);
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

            if (currentPrize.File != null && prize.File == null)
            {
                Uow.FileContents.Delete(currentPrize.File.Id);
                Uow.Files.Delete(currentPrize.File.Id);
            }
            else if (currentPrize.File == null && prize.File != null)
            {
                //Add new image
                prize.File.CreatedDate = _clock.Now;
                currentPrize.File = prize.File;
            }
            else if (currentPrize.File != null && prize.File != null && prize.File.FileContent != null && prize.File.FileContent.Content != null)
            {
                //Edit actual
                currentPrize.File.Name = prize.File.Name;
                currentPrize.File.ContentType = prize.File.ContentType;
                currentPrize.File.ModifiedDate = _clock.Now;

                //Edit content
                prize.File.FileContent.Id = currentPrize.File.Id;
                Uow.FileContents.Edit(prize.File.FileContent);
            }

            currentPrize.ModifiedDate = _clock.Now;
            currentPrize.Name = prize.Name;
            currentPrize.PointsNeeded = prize.PointsNeeded;
            currentPrize.Description = prize.Description;

            Uow.Prizes.Edit(currentPrize);
            Uow.Commit();
        }

        public void Delete(int prizeId)
        {
            var currentPrize = GetById(prizeId);

            if (currentPrize.File != null)
            {
                Uow.FileContents.Delete(currentPrize.File.Id);
                Uow.Files.Delete(currentPrize.File.Id);
            }

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
