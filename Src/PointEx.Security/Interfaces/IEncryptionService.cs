namespace PointEx.Security.Interfaces
{
    public interface IEncryptionService
    {
        string CalculateHash(string clearTextPassword, string salt);
    }
}
