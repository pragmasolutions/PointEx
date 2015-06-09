using System.Web.Mvc;
using PointEx.Security;
using PointEx.Web.Controllers;

namespace PointEx.Web.Areas.Beneficiary.Controllers
{
    [Authorize(Roles = RolesNames.Beneficiary)]
    public abstract class BeneficiaryBaseController : BaseController
    {
    }
}
