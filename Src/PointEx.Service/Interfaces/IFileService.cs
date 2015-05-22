using System.Linq;
using PointEx.Entities;

namespace PointEx.Service
{
    public interface IFileService
    {
        File GetById(int id);
    }
}