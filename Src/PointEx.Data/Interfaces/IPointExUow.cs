using System;
using Framework.Data.Repository;
using PointEx.Entities;

namespace PointEx.Data.Interfaces
{
    public interface IPointExUow : IUow
    {
        IRepository<Student> Students { get; }
        IRepository<Shop> Shops { get; }
        IRepository<Town> Towns { get; }
    }
}
