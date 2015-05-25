using System;
using System.Linq.Expressions;
using System.Web.Mvc;
using Microsoft.Web.Mvc;
using PointEx.Security;

namespace PointEx.Web.Controllers
{
    [Authorize(Roles = RolesNames.Shop)]
    public abstract class ShopBaseController : BaseController
    {
    }
}
