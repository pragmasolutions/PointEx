using System.Collections.Generic;
using System.Linq;
using PointEx.Entities;
using PointEx.Entities.Dto;

namespace PointEx.Service
{
    public interface IPrizeService
    {
        IQueryable<Prize> GetAll();

        Prize GetById(int id);

        List<PrizeDto> GetAll(string sortBy, string sortDirection, string criteria, int? maxPointsNeeded,
            int pageIndex, int pageSize, out int pageTotal);

        void Create(Prize prize);

        void Edit(Prize prize);

        void Delete(int prizeId);

        bool IsNameAvailable(string name, int id);
    }
}