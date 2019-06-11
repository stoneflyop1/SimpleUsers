using System.Threading.Tasks;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SimpleUsers.Core.Entities;
using SimpleUsers.Core.Models;

namespace SimpleUsers.Core.Services
{
    /// <summary>
    /// 用户相关接口
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task RegisterAsync(RegisterModel model);
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UserDto> LoginAsync(string userName, string password);
        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateInfoAsync(string userId, UserInfoModel model);
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<UserDto> GetInfoAsync(string userId);
    }

#pragma warning disable 1591
// https://github.com/dotnet-architecture/eShopOnContainers/issues/515
// https://blog.stephencleary.com/2017/03/aspnetcore-synchronization-context.html
// https://thinkrethink.net/2017/10/30/asp-net-core-correct-usage-of-configureawait-with-asyncawait/
#pragma warning disable CA2007

    public class UserService : IUserService
    {
        private readonly DbContext _db;
        private readonly IPasswordHasher _passHasher;
        private readonly ILogger _log;

        public UserService(DbContext db, IPasswordHasher passHasher, ILogger<UserService> logger)
        {
            _db = db;
            _passHasher = passHasher;
            _log = logger;
        }

        public async Task<UserDto> GetInfoAsync(string userId)
        {
            _log.LogInformation($"Get UserInfo: {userId}");
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
            _log.LogInformation($"User Login: {userName}");
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
            if (model == null) throw new ArgumentNullException(nameof(model));
            _log.LogInformation($"User Register: {model.UserName}");
            var passHash = _passHasher.HashPassword(model.Password);
            var user = new User{UserName = model.UserName, PasswordHash = passHash};
            await _db.Set<User>().AddAsync(user);
            await _db.SaveChangesAsync();
        }

        public async Task UpdateInfoAsync(string userId, UserInfoModel model)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (model == null) throw new ArgumentNullException(nameof(model));
            _log.LogInformation($"Update UserInfo: {userId}");
            var user = await _db.Set<User>().FindAsync(userId);
            if (user == null) return;
            user.Email = model.Email;
            user.Mobile = model.Mobile;
            user.Name = model.Name;
            await _db.SaveChangesAsync();
        }
    }
}