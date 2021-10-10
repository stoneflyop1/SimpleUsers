using System;
using Xunit;
using SimpleUsers.Core.Services;
using Microsoft.EntityFrameworkCore;
using SimpleUsers.Core;
using SimpleUsers.Core.Models;
using SimpleUsers.Core.Entities;
using Microsoft.Extensions.Logging;

namespace SimpleUsers.Tests
{
    public class UserTests
    {
        private readonly IPasswordHasher _hasher = new PasswordHasher();

        private readonly DbContext _db;
        private readonly IUserService _userService;
        private readonly ILoggerFactory _logFactory;

        public UserTests()
        {
            // https://garywoodfine.com/entity-framework-core-memory-testing-database/
            var options = new DbContextOptionsBuilder<UserContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging().Options;
            _db = new UserContext(options);
            _logFactory = new LoggerFactory();
            _userService = new UserService(_db, _hasher, _logFactory.CreateLogger<UserService>());
        }

        [Fact]
        public void User_OK()
        {
            var userName = "test";
            var password = "123456";

            // Register
            var model = new RegisterModel{
                UserName = userName,
                Password = password
            };
            _userService.RegisterAsync(model).Wait();
            var user = _db.Set<User>().FirstOrDefaultAsync(u => u.UserName == userName).Result;
            Assert.NotNull(user);

            // Login
            var dto = _userService.LoginAsync(userName, password).Result;
            Assert.NotNull(dto);
            Assert.Equal(userName, dto.UserName);


            // UserInfo
            var findedUser = _userService.GetInfoAsync(user.Id).Result;
            Assert.NotNull(findedUser);
            Assert.Equal(userName, findedUser.UserName);

            var email = "test@test.com";
            var fullName = "测试";
            var userInfo = new UserInfoModel(){
                Name = fullName,
                Email = email
            };
            _userService.UpdateInfoAsync(user.Id, userInfo).Wait();

            var findedUser2 = _userService.GetInfoAsync(user.Id).Result;
            Assert.NotNull(findedUser2);
            Assert.Equal(email, findedUser2.Email);
            Assert.Equal(fullName, findedUser2.Name);
        }
    }
}
