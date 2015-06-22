using System.Web.Mvc;
using PointEx.Security;
using PointEx.Web.Controllers;

namespace PointEx.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Administrator,SuperAdmin")]
    public abstract class AdminBaseController : BaseController
    {
    }
}
