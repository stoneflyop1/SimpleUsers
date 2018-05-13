using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName {get;set;}
        [Required]
        [MinLength(6)]
        public string Password {get;set;}
    }
}