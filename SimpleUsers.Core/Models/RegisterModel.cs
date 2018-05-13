using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    /// <summary>
    /// 注册的ViewModel
    /// </summary>
    public class RegisterModel
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [Display(Name="用户名")]
        [Required]
        public string UserName {get;set;}
        /// <summary>
        /// 密码
        /// </summary>
        [Display(Name="密码")]
        [Required]
        [MinLength(6)]
        public string Password {get;set;}
    }
}