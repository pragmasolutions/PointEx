using Framework.Common.Utility;
using Framework.Data.EntityFramework.Helpers;
using Ninject;
using Ninject.Web.Common;
using PointEx.Data;
using PointEx.Data.Interfaces;
using PointEx.Service;

namespace PointEx.Web
{
    /// <summary>
    /// Helper class to register all global config of ninject.
    /// </summary>
    public class IoCConfig
    {
        /// <summary>
        /// Register all the binding.
        /// </summary>
        /// <param name="kernel"></param>
        public static void RegisterBindings(IKernel kernel)
        {
            kernel.Bind<RepositoryFactories>().To<RepositoryFactories>().InSingletonScope();
            kernel.Bind<IClock>().To<Clock>().InSingletonScope();
            kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            kernel.Bind<IPointExUow>().To<PointExUow>().InRequestScope();
            kernel.Bind<IShopService>().To<ShopService>().InRequestScope();
            kernel.Bind<ITownService>().To<TownService>().InRequestScope();
            kernel.Bind<ICategoryService>().To<CategoryService>().InRequestScope(); 
        }
    }
}
