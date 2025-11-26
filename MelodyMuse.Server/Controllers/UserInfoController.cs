using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Controllers.Facades;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TencentCloud.Dlc.V20210125.Models;

namespace MelodyMuse.Server.Controllers
{

    //启用接收数据自动绑定
    [ApiController]
    //注册/api/users分支路由
    [Route("api/userinfo")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUsersService _usersService;
        private readonly UserContextFacade _userContext;

        //构造函数:初始化(传入相应的服务)接口
        public UserInfoController(IUsersService usersService, UserContextFacade userContext)
        {
            _usersService = usersService;
            _userContext = userContext;
        }

        //获取当前用户
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult> GetNowUserInfo()
        {
            try
            {
                var parsedToken = _userContext.GetCurrentUserTokenInfo();

                //构建实例
                UserModel user = await _usersService.GetUserById(parsedToken.UserID);
                if (user == null)
                {
                    // 不存在时的处理逻辑
                    return NotFound(new { msg = "用户不存在" });
                }
                //检查是否为歌手
                if (await _usersService.Useridentity("user" + parsedToken.UserID))
                {
                    return Ok(new
                    {
                       UserID=user.UserId,
                       UserName=user.UserName,
                       UserEmail=user.UserEmail,
                       UserPhone=user.UserPhone,
                       UserSex=user.UserSex,
                       UserAge=user.UserAge,
                       UserBirthday=user.UserBirthday,
                       UserStatus=user.UserStatus,
                       IsArtist=true,
                       ArtistId = "user" + user.UserId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        UserID = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                        UserPhone = user.UserPhone,
                        UserSex = user.UserSex,
                        UserAge = user.UserAge,
                        UserBirthday = user.UserBirthday,
                        UserStatus = user.UserStatus,
                        IsArtist = false
                    });
                }

               
            }
            catch (Exception ex)
            {
                // 捕获异常并返回服务器错误
                return StatusCode(500, new { msg = "服务器错误", error = ex.Message });
            }
        }


        //获取其他用户
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<ActionResult> GetOtherUserInfo(string userId)
        {
            try
            {

                //构建实例
                UserModel user = await _usersService.GetUserById(userId);
                if (user == null)
                {
                    // 不存在时的处理逻辑
                    return NotFound(new { msg = "用户不存在" });
                }
                //检查是否为歌手
                if (await _usersService.Useridentity("user" + userId))
                {
                    return Ok(new
                    {
                        UserID = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                        UserPhone = user.UserPhone,
                        UserSex = user.UserSex,
                        UserAge = user.UserAge,
                        UserBirthday = user.UserBirthday,
                        UserStatus = user.UserStatus,
                        IsArtist = true,
                        ArtistId = "user" + user.UserId
                    });
                }
                else
                {
                    return Ok(new
                    {
                        UserID = user.UserId,
                        UserName = user.UserName,
                        UserEmail = user.UserEmail,
                        UserPhone = user.UserPhone,
                        UserSex = user.UserSex,
                        UserAge = user.UserAge,
                        UserBirthday = user.UserBirthday,
                        UserStatus = user.UserStatus,
                        IsArtist = false
                    });
                }


            }
            catch (Exception ex)
            {
                // 捕获异常并返回服务器错误
                return StatusCode(500, new { msg = "服务器错误", error = ex.Message });
            }
        }
    }
}
