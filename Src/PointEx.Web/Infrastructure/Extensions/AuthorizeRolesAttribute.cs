using System.Web.Mvc;

namespace PointEx.Web.Infrastructure.Extensions
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
       public AuthorizeRolesAttribute(params string[] roles) : base()
        {
            Roles = string.Join(",", roles);
        }
    }
}
