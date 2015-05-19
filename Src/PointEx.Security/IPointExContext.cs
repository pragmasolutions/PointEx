using PointEx.Entities;

namespace PointEx.Security
{
    public interface IPointExContext
    {
        User User { get; }
        Beneficiary Beneficiary { get; }
        bool IsInRole(string roles);
        string Role { get; }
    }
}
