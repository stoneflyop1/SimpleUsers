namespace SimpleUsers.Core.Entities
{
    /// <summary>
    /// 用户
    /// </summary>
    public class User
    {
        /// <summary>
        /// 用户id
        /// </summary>
        /// <returns></returns>
        public string Id {get;set;}
        /// <summary>
        /// 用户名
        /// </summary>
        /// <returns></returns>
        public string UserName {get;set;}
        /// <summary>
        /// 密码哈希
        /// </summary>
        /// <returns></returns>
        public string PasswordHash {get;set;}
        /// <summary>
        /// 姓名
        /// </summary>
        /// <returns></returns>
        public string Name {get;set;}
        /// <summary>
        /// 手机号
        /// </summary>
        /// <returns></returns>
        public string Mobile {get;set;}
        /// <summary>
        /// 邮箱
        /// </summary>
        /// <returns></returns>
        public string Email {get;set;}
    }
}