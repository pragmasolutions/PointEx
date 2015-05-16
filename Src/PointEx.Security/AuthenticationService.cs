using System;
using PointEx.Data.Interfaces;
using PointEx.Entities;
using PointEx.Security.Interfaces;

namespace PointEx.Security
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IPointExUow _uow;
        private readonly IEncryptionService _encryptionService;

        public AuthenticationService(IPointExUow uow, IEncryptionService encryptionService)
        {
            _uow = uow;
            _encryptionService = encryptionService;
        }

        public User AuthenticateUser(string username, string clearTextPassword)
        {
            var hashPassword = _encryptionService.CalculateHash(clearTextPassword, username);
            var user =
                _uow.Users.Get(
                    u => u.UserName.ToUpper().Equals(username) && u.PasswordHash.Equals(hashPassword),
                    u => u.Roles);

            if (user == null)
                throw new UnauthorizedAccessException("Access denied. Please provide some valid credentials.");

            return user;
        }
    }
}
