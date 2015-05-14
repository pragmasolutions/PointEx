using System;

namespace Framework.Data.Repository
{
    public interface IUow : IDisposable
    {
        void Commit();
    }
}
