namespace SimpleUsers.Core.Services
{
    public interface IPasswordHasher
    {
        string HashPassword(string password);

        bool VerifyPassword(string password, string passwordHash);
    }

#pragma warning disable 1591

    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            return password;
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            return password == passwordHash;
        }
    }
}