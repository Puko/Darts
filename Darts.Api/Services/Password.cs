using Darts.Domain.Abstracts;

namespace Darts.Api.Services
{
    public class Password : IPassword
    {
        public string Hash(string plainPassword)
        {
            return BCrypt.Net.BCrypt.HashPassword(plainPassword);
        }

        public bool Verify(string password, string hash)
        {
            return BCrypt.Net.BCrypt.Verify(password, hash);
        }
    }
}
