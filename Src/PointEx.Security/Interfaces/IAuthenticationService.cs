using PointEx.Entities;

namespace PointEx.Security.Interfaces
{
    public interface IAuthenticationService
    {
        User AuthenticateUser(string username, string password);
    }
}
