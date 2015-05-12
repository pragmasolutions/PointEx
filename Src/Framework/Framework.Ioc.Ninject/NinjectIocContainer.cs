using System;
using Ninject;
using System.Collections.Generic;

namespace Framework.Ioc
{
    public class NinjectIocContainer : IIocContainer
    {
        private readonly IKernel _kernel;

        public NinjectIocContainer(IKernel kernel)
        {
            _kernel = kernel;
        }

        public object Get(Type type)
        {
            return _kernel.Get(type);
        }

        public T TryGet<T>()
        {
            return _kernel.TryGet<T>();
        }

        public T Get<T>()
        {
            return _kernel.Get<T>();
        }

        public T Get<T>(string name, string value)
        {
            var result = _kernel.TryGet<T>(metadata => metadata.Has(name) &&
                         (string.Equals(metadata.Get<string>(name), value,
                                        StringComparison.InvariantCultureIgnoreCase)));

            return result;
        }

        public IEnumerable<T> GetAll<T>()
        {
            return _kernel.GetAll<T>();
        }

        public void Inject(object item)
        {
            _kernel.Inject(item);
        }
    }
}
