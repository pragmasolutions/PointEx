using System.Web.Mvc;
using PointEx.Security;
using PointEx.Web.Controllers;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = RolesNames.Admin)]
    public abstract class AdminBaseController : BaseController
    {
    }
}
