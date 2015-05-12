using System;

namespace Framework.Data.Repository
{
    public interface ILaPazUow : IDisposable
    {
        void Commit();
    }
}
