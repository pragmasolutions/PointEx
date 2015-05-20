using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Framework.Common.Web.ModelBinders;
using Framework.Ioc;
using Framework.Ioc.Ninject;
using Ninject;
using Resources;

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

            ModelBinderProviders.BinderProviders.Add(new EFModelBinderProvider());

            IoCConfig.Config();

            AutoMapperConfig.Config();

            ValidationConfig.Config();
        }
    }
}
