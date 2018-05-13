using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    /// <summary>
    /// 登录的ViewModel
    /// </summary>
    public class LoginModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name = "用户名")]
        [Required]
        public string UserName {get;set;}
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name = "密码")]
        [Required]
        public string Password {get;set;}
    }
}