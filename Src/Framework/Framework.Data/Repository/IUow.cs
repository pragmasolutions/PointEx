using System;
using System.Threading.Tasks;

namespace Framework.Data.Repository
{
    public interface IUow : IDisposable
    {
        void Commit();
        Task CommitAsync();
    }
}
