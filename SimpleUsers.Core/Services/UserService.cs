using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SimpleUsers.Core.Entities;
using SimpleUsers.Core.Models;

namespace SimpleUsers.Core.Services
{
    public interface IUserService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task RegisterAsync(RegisterModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserDto> LoginAsync(string userName, string password);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateInfoAsync(string userId, UserInfoModel model);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetInfoAsync(string userId);
    }

#pragma warning disable 1591

    public class UserService : IUserService
    {
        private readonly DbContext _db;
        private readonly IPasswordHasher _passHasher;

        public UserService(DbContext db, IPasswordHasher passHasher)
        {
            _db = db;
            _passHasher = passHasher;
        }

        public async Task<UserDto> GetInfoAsync(string userId)
        {
            var user = await _db.Set<User>().FindAsync(userId);
            if (user == null) return null;
            return new UserDto{
                UserId = userId, 
                UserName = user.UserName, 
                Name = user.Name, 
                Mobile = user.Mobile, 
                Email = user.Email
            };
        }

        public async Task<UserDto> LoginAsync(string userName, string password)
        {
            var passHash = _passHasher.HashPassword(password);
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => 
                u.UserName == userName && u.PasswordHash == passHash);
            if (user == null) return null;
            return new UserDto{
                UserId = user.Id, 
                UserName = user.UserName, 
                Name = user.Name, 
                Mobile = user.Mobile, 
                Email = user.Email
            };
        }

        public async Task RegisterAsync(RegisterModel model)
        {
            var passHash = _passHasher.HashPassword(model.Password);
            var user = new User{UserName = model.UserName, PasswordHash = passHash};
            await _db.Set<User>().AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateInfoAsync(string userId, UserInfoModel model)
        {
            var user = await _db.Set<User>().FindAsync(userId);
            if (user == null) return;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            await _db.SaveChangesAsync();
        }
    }
}