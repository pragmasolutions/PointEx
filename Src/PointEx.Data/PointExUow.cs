using System;
using System.Configuration;
using System.Data.Entity.Core.EntityClient;
using System.Threading.Tasks;
using Framework.Data.EntityFramework.Helpers;
using Framework.Data.Repository;
using PointEx.Data.Interfaces;
using PointEx.Entities;

namespace PointEx.Data
{
    public class PointExUow : IPointExUow
    {
        public PointExUow(IRepositoryProvider repositoryProvider)
        {
            CreateDbContext();

            repositoryProvider.DbContext = DbContext;
            RepositoryProvider = repositoryProvider;
        }

        public IRepository<Beneficiary> Beneficiaries { get { return GetStandardRepo<Beneficiary>(); } }
        public IRepository<Shop> Shops { get { return GetStandardRepo<Shop>(); } }
        public IRepository<Town> Towns { get { return GetStandardRepo<Town>(); } }
        public IRepository<User> Users { get { return GetStandardRepo<User>(); } }
        public IRepository<Role> Roles { get { return GetStandardRepo<Role>(); } }
        public IRepository<ShopCategory> ShopCategories { get { return GetStandardRepo<ShopCategory>(); } }
        public IRepository<Category> Categories { get { return GetStandardRepo<Category>(); } }
        public IRepository<EducationalInstitution> EducationalInstitutions { get { return GetStandardRepo<EducationalInstitution>(); } }
        public IRepository<Card> Cards { get { return GetStandardRepo<Card>(); }}
        public IRepository<Prize> Prizes { get { return GetStandardRepo<Prize>(); }}
        public IRepository<PointsExchange> PointsExchanges { get { return GetStandardRepo<PointsExchange>(); } }
        public IRepository<File> Files { get { return GetStandardRepo<File>(); } }
        public IRepository<FileContent> FileContents { get { return GetStandardRepo<FileContent>(); } }
        public IRepository<Benefit> Benefits { get { return GetStandardRepo<Benefit>(); } }
        public IRepository<Purchase> Purchases { get { return GetStandardRepo<Purchase>(); } }
        public IRepository<BenefitFile> BenefitFiles { get { return GetStandardRepo<BenefitFile>(); } }
        

        public string ConnectionString
        {
            get
            {
                var builder = new EntityConnectionStringBuilder();
                builder.Metadata = @"res://*/PointExModel.csdl|res://*/PointExModel.ssdl|res://*/PointExModel.msl";
                builder.Provider = "System.Data.SqlClient";
                builder.ProviderConnectionString = ConfigurationManager.ConnectionStrings["PointExDbContext"].ConnectionString;
                return builder.ToString();
            }
        }

        /// <summary>
        /// Save pending changes to the database
        /// </summary>
        public void Commit()
        {
            DbContext.SaveChanges();
        }

        public Task CommitAsync()
        {
            return DbContext.SaveChangesAsync();
        }

        protected void CreateDbContext()
        {
            DbContext = new PointExDbContext(ConnectionString);

            // Do NOT enable proxied entities, else serialization fails
            DbContext.Configuration.ProxyCreationEnabled = false;

            // Load navigation properties explicitly (avoid serialization trouble)
            DbContext.Configuration.LazyLoadingEnabled = false;

            // Because Web API will perform validation, we don't need/want EF to do so
            DbContext.Configuration.ValidateOnSaveEnabled = false;

            //DbContext.Configuration.AutoDetectChangesEnabled = false;
            // We won't use this performance tweak because we don't need 
            // the extra performance and, when autodetect is false,
            // we'd have to be careful. We're not being that careful.
        }

        protected IRepositoryProvider RepositoryProvider { get; set; }

        private IRepository<T> GetStandardRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepositoryForEntityType<T>();
        }

        private T GetRepo<T>() where T : class
        {
            return RepositoryProvider.GetRepository<T>();
        }

        public PointExDbContext DbContext { get; set; }

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DbContext != null)
                {
                    DbContext.Dispose();
                }
            }
        }

        #endregion


        
    }
}
