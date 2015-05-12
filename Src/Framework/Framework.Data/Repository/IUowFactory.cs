namespace Framework.Data.Repository
{
    public interface IUowFactory
    {
        T Create<T>();
    }
}
