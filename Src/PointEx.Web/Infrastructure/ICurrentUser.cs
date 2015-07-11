using PointEx.Entities;

namespace PointEx.Web.Infrastructure
{
	public interface ICurrentUser
	{
        Shop Shop { get; }
        Beneficiary Beneficiary { get; }
	}
}