namespace Framework.Security.Interfaces
{
    public interface IAuthenticationService
    {
        bool AuthenticateUser(string username, string password);
    }
}
