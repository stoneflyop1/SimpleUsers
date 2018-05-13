using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    public class LoginModel
    {
        [Display(Name = "用户名")]
        [Required]
        public string UserName {get;set;}
        [Display(Name = "密码")]
        [Required]
        public string Password {get;set;}
    }
}