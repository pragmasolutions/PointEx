using System;
using System.Collections.Generic;

namespace Framework.Ioc
{
    public static class Ioc
    {
        public static IIocContainer Container { get; set; }
    }

    public interface IIocContainer
    {
        object Get(Type type);
        T Get<T>();
        T Get<T>(string name, string value);
        void Inject(object item);
        T TryGet<T>();
        IEnumerable<T> GetAll<T>();
    }
}
