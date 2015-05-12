using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PointEx.Web.Startup))]
namespace PointEx.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
