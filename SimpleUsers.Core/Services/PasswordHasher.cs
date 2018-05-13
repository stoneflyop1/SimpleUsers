namespace SimpleUsers.Core.Services
{
    /// <summary>
    /// 密码哈希接口
    /// </summary>
    public interface IPasswordHasher
    {
        /// <summary>
        /// 对密码进行哈希
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        string HashPassword(string password);
        /// <summary>
        /// 验证密码哈希
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <returns></returns>
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