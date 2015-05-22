using System;
using Framework.Data.Repository;
using PointEx.Entities;

namespace PointEx.Data.Interfaces
{
    public interface IPointExUow : IUow
    {
        IRepository<Beneficiary> Beneficiaries { get; }
        IRepository<Shop> Shops { get; }
        IRepository<Town> Towns { get; }
        IRepository<User> Users { get; }
        IRepository<Role> Roles { get; }
        IRepository<Category> Categories { get; }
        IRepository<ShopCategory> ShopCategories { get; }
        IRepository<EducationalInstitution> EducationalInstitutions { get; }
        IRepository<Card> Cards { get; }
        IRepository<Prize> Prizes { get; }
        IRepository<PointsExchange> PointsExchanges { get; }
        IRepository<File> Files { get; }
        IRepository<FileContent> FileContents { get; }
    }
}
