using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface ITownService
    {
        void Create(Town town);

        IQueryable<Town> GetAll();

        Town GetById(int id);

        Town GetByName(string name);
    }
}