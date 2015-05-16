using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Framework.Common.Web.ModelBinders;
using Framework.Ioc;
using Framework.Ioc.Ninject;
using Ninject;

namespace PointEx.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfig.Config();

            var kernel = new StandardKernel();

            ControllerBuilder.Current.SetControllerFactory(new NinjectControllerFactory(kernel));

            IoCConfig.RegisterBindings(kernel);

            IocContainer.Initialize(new NinjectIocContainer(kernel));

            ModelBinderProviders.BinderProviders.Add(new EFModelBinderProvider());
        }
    }
}
