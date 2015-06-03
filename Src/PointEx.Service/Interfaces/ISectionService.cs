using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PointEx.Entities;
using PointEx.Entities.Dto;
using PointEx.Security.Model;

namespace PointEx.Service
{
    public interface ISectionService : IServive
    {
        IList<Section> GetAll();
        Section GetById(int id);
        Section GetByName(string name);
        void Dispose();
    }
}