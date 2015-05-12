using System;
using System.Data.Entity;

namespace Framework.Data.EntityFramework.Repository
{
    /// <summary>
    /// The EF-dependent, base repository for data access
    /// </summary>
    public class EFBaseRepository
    {
        public EFBaseRepository(DbContext dbContext)
        {
            if (dbContext == null) 
                throw new ArgumentNullException("dbContext");
            DbContext = dbContext;
        }

        protected DbContext DbContext { get; set; }
    }
}
