using PointEx.Entities;
using PointEx.Security.Model;

namespace PointEx.Web.Infrastructure
{
	public interface ICurrentUser
	{
        Shop Shop { get; }
        Beneficiary Beneficiary { get; }
        User PointexUser { get; }
        ApplicationUser User { get; }
	}
}