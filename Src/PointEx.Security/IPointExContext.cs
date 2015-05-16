using PointEx.Entities;

namespace PointEx.Security
{
    public interface IPointExContext
    {
        User User { get; }
        bool IsInRole(string roles);
        Role Role { get; }
    }
}
