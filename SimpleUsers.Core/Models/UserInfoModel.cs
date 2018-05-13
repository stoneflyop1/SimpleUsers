using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    /// <summary>
    /// 用户信息的ViewModel
    /// </summary>
    public class UserInfoModel
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [Display(Name="姓名")]
        public string Name {get;set;}
        /// <summary>
        /// 手机号
        /// </summary>
        [Display(Name="手机号")]
        public string Mobile {get;set;}
        /// <summary>
        /// 邮箱
        /// </summary>
        [Display(Name="邮箱")]
        [EmailAddress]
        public string Email {get;set;}
    }
}