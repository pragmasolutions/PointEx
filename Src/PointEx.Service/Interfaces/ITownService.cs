using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ITownService
    {
        IQueryable<Town> GetAll();

        Town GetById(int id);
    }
}