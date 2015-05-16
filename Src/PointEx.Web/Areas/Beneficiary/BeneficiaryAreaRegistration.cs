using System.Web.Mvc;

namespace PointEx.Web.Areas.Beneficiary
{
    public class BeneficiaryAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Beneficiary";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Beneficiary_default",
                "Beneficiary/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}