using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Mvc;
using Framework.Common.Utility;
using Framework.Data.EntityFramework.Helpers;
using Framework.Ioc;
using Framework.Ioc.Ninject;
using Microsoft.AspNet.Identity.Owin;
using Ninject;
using Ninject.Web.Common;
using PointEx.Data;
using PointEx.Data.Interfaces;
using PointEx.Security.Managers;
using PointEx.Service;
using PointEx.Web.Infrastructure;

namespace PointEx.Web
{
    /// <summary>
    /// Helper class to register all global config of ninject.
    /// </summary>
    public class IoCConfig
    {
        public static void Config(StandardKernel kernel)
        {
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
            kernel.Bind<IPrizeService>().To<PrizeService>().InRequestScope();
            kernel.Bind<IFileService>().To<FileService>().InRequestScope();
            kernel.Bind<IBenefitService>().To<BenefitService>().InRequestScope();
            kernel.Bind<IPurchaseService>().To<PurchaseService>().InRequestScope();
            kernel.Bind<ICardService>().To<CardService>().InRequestScope();
            kernel.Bind<IBenefitFileService>().To<BenefitFileService>().InRequestScope();

            kernel.Bind<ICurrentUser>().To<CurrentUser>().InRequestScope();
            kernel.Bind<IIdentity>().ToMethod(c => HttpContext.Current.User.Identity);
        }
    }
}
