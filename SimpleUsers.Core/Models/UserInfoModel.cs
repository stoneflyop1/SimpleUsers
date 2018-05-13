using System.ComponentModel.DataAnnotations;

namespace SimpleUsers.Core.Models
{
    public class UserInfoModel
    {
        public string Name {get;set;}

        public string Mobile {get;set;}
        [EmailAddress]
        public string Email {get;set;}
    }
}