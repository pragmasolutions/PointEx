using System.Web;
using System.Web.Mvc;
using Framework.Common.Utility;
using Framework.Data.EntityFramework.Helpers;
using Framework.Ioc;
using Framework.Ioc.Ninject;
using Ninject;
using Ninject.Web.Common;
using PointEx.Data;
using PointEx.Data.Interfaces;
using PointEx.Service;
using Ninject.Extensions.Conventions;
using PointEx.Security.Managers;
using System.Web;
using Microsoft.AspNet.Identity.Owin;

namespace PointEx.Web
{
    /// <summary>
    /// Helper class to register all global config of ninject.
    /// </summary>
    public class IoCConfig
    {
        public static void Config()
        {
            var kernel = new StandardKernel();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));

            RegisterBindings(kernel);

            IocContainer.Initialize(new NinjectIocContainer(kernel));
        }

        /// <summary>
        /// Register all the binding.
        /// </summary>
        /// <param name="kernel"></param>
        private static void RegisterBindings(IKernel kernel)
        {
            kernel.Bind<RepositoryFactories>().To<RepositoryFactories>().InSingletonScope();
            kernel.Bind<IClock>().To<Clock>().InSingletonScope();
            kernel.Bind<IRepositoryProvider>().To<RepositoryProvider>();
            kernel.Bind<IPointExUow>().To<PointExUow>().InRequestScope();

            kernel.Bind<ApplicationUserManager>().ToMethod(c => HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>());
            kernel.Bind<ApplicationSignInManager>().ToMethod(c => HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>());

            kernel.Bind<IShopService>().To<ShopService>().InRequestScope();
            kernel.Bind<ITownService>().To<TownService>().InRequestScope();
            kernel.Bind<ICategoryService>().To<CategoryService>().InRequestScope();
            kernel.Bind<IBeneficiaryService>().To<BeneficiaryService>().InRequestScope();
            kernel.Bind<IEducationalInstitutionService>().To<EducationalInstitutionService>().InRequestScope();
        }
    }
}
