using System;
using System.Collections.Generic;
using System.Data.Entity;
using Framework.Data.EntityFramework.Helpers;
using Framework.Data.EntityFramework.Repository;
using Framework.Data.Repository;
using PointEx.Data.Interfaces;
using PointEx.Data.Repository;

namespace PointEx.Data.Helpers
{
    public class PointExRepositoryFactories : RepositoryFactories
    {
        protected override IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
             {
                  {typeof(IReportRepository), dbContext => new ReportRepository(dbContext)}
              };
        }
    }
}
