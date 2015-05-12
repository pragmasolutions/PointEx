using System;
using System.Collections.Generic;
using System.Data.Entity;
using Framework.Data.EntityFramework.Helpers;

namespace PointEx.Data.Helpers
{
    public class PointExRepositoryFactories : RepositoryFactories
    {
        protected override IDictionary<Type, Func<DbContext, object>> GetFactories()
        {
            return new Dictionary<Type, Func<DbContext, object>>
             {
                  //{typeof(ICustomRepoRepository), dbContext => new ReporteRepository(dbContext)}
              };
        }
    }
}
