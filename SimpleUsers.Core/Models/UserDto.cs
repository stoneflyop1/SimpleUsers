namespace SimpleUsers.Core.Models
{
    /// <summary>
    /// 用户信息DTO
    /// </summary>
    public class UserDto
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserId {get;set;}
        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName {get;set;}
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name {get;set;}
        /// <summary>
        /// 手机号
        /// </summary>
        public string Mobile {get;set;}
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email {get;set;}
    }
}