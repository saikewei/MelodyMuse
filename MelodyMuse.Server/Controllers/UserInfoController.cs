using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;

namespace MelodyMuse.Server.Controllers
{

    //启用接收数据自动绑定
    [ApiController]
    //注册/api/users分支路由
    [Route("api/userinfo")]
    public class UserInfoController : ControllerBase
    {
        private readonly IUsersService _usersService;

        //构造函数:初始化(传入相应的服务)接口
        public UserInfoController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        //获取当前用户
        [Authorize]
        [HttpGet("info")]
        public async Task<ActionResult> GetNowUserInfo()
        {
            try
            {
                // 从请求头中获取 JWT 令牌
                var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

                // 如果没有令牌，返回未授权错误码401
                if (token == null)
                {
                    return Unauthorized(new { msg = "未提供令牌" });
                }

                // 解析 JWT 令牌，得到存储的信息
                var parsedToken = TokenParser.ParseToken(token, JWTConfigure.serect_key);

                // 检查解析结果是否为空
                if (parsedToken == null)
                {
                    return Unauthorized(new { msg = "令牌无效" });
                }

                // 输出测试查看是否正确
                Console.WriteLine(parsedToken.UserID + " " + parsedToken.Username + " " + parsedToken.UserPhone);

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
                        User = user,
                        ArtistId = "user" + user.UserId
                    });
                }
                else
                {
                    return Ok(user);
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
