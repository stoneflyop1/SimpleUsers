using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using SimpleUsers.Core.Models;
using SimpleUsers.Core.Services;
using SimpleUsers.WebAPI.Providers;

namespace SimpleUsers.WebAPI.Controllers
{
    /// <summary>
    /// 用户相关操作入口
    /// </summary>
    [Authorize]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger _logger;

#pragma warning disable 1591
        public UserController(IUserService userService, ILoggerFactory logFactory)
        {
            _userService = userService;
            _logger = logFactory.CreateLogger<UserController>();
        }
#pragma warning restore 1591

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="model">注册实体模型</param>
        /// <returns></returns>
        [Route("Register")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult> Register([FromBody]RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResultUtil.Fail("数据不合法!");
            }
            try
            {
                await _userService.RegisterAsync(model);
                return ApiResultUtil.Success();
            }
            catch(Exception ex)
            {
                _logger.LogError(0, ex, "注册失败："+ApiResultUtil.GetMessage(ex));
                return ApiResultUtil.Fail("注册失败！");
            }            
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model">登陆实体模型</param>
        /// <returns></returns>
        [Route("Login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<ApiResult<TokenEntity>> Login([FromBody]LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return ApiResultUtil.Fail<TokenEntity>("数据不合法!");
            }
            var userName = model.UserName;
            var password = model.Password;
            try
            {
                var user = await _userService.LoginAsync(userName, password);
                if (user == null)
                {
                    return ApiResultUtil.Fail<TokenEntity>("用户名或密码不正确!");
                }
                var tp = TokenUtil.GetProvider();
                var token = tp.GenerateToken(user);

                return ApiResultUtil.Success(token);
            }
            catch(Exception ex)
            {
                _logger.LogError(0, ex, "登录失败："+ApiResultUtil.GetMessage(ex));
                return ApiResultUtil.Fail<TokenEntity>("登录失败！");
            }
            
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("UserInfo")]
        [HttpPost]
        public async Task<ApiResult> UpdateUserInfo([FromBody]UserInfoModel model)
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return ApiResultUtil.Fail<UserDto>("无法获取Identity信息！");
            }
            var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Prn);
            if (userIdClaim == null)
            {
                return ApiResultUtil.Fail<UserDto>("无法获取Identity信息");
            }

            var userId = userIdClaim.Value;
            try
            {
                await _userService.UpdateInfoAsync(userId, model);

                return ApiResultUtil.Success();
            }
            catch(Exception ex)
            {
                _logger.LogError(0, ex, "更新用户信息失败："+ApiResultUtil.GetMessage(ex));
                return ApiResultUtil.Fail<TokenEntity>("更新用户信息失败！");
            }
            
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        [Route("UserInfo")]
        [HttpGet]
        public async Task<ApiResult<UserDto>> GetUserInfo()
        {
            var identity = User.Identity as ClaimsIdentity;

            if (identity == null)
            {
                return ApiResultUtil.Fail<UserDto>("无法获取Identity信息！");
            }
            var userIdClaim = identity.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Prn);
            if (userIdClaim == null)
            {
                return ApiResultUtil.Fail<UserDto>("无法获取Identity信息");
            }
            try
            {
                var user = await _userService.GetInfoAsync(userIdClaim.Value);
                return ApiResultUtil.Success(user);
            }
            catch(Exception ex)
            {
                _logger.LogError(0, ex, "获取用户信息失败："+ApiResultUtil.GetMessage(ex));
                return ApiResultUtil.Fail<UserDto>("获取用户信息失败！");
            }
            
        }
    }
}
