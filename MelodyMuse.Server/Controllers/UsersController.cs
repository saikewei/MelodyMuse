/*
与管理员管理用户有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;
using Microsoft.AspNetCore.Authorization;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;

//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定
    [ApiController]
    //注册/api/users分支路由
    [Route("api/users")]


    public class UsersController : ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly IUsersService _usersService;

        //构造函数:初始化(传入相应的服务)接口
        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        //获取当前用户
        [Authorize]
        [HttpGet("getNow")]
        public async Task<ActionResult> GetNowUser()
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

                // 这里你可以返回解析出来的用户信息或其他相关数据
                return Ok(new { userId = parsedToken.UserID, 
                    username = parsedToken.Username, 
                    userPhone = parsedToken.UserPhone });
            }
            catch (Exception ex)
            {
                // 捕获异常并返回服务器错误
                return StatusCode(500, new { msg = "服务器错误", error = ex.Message });
            }
        }

        //获取所有用户id列表
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<string>>> GetAllUserIds()
        {
            try
            {
                var userIds = await _usersService.GetAllUserIds();
                return Ok(userIds);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "查询所有用户ID失败: " + ex.Message
                };
                return StatusCode(500, errorResponse);
            }
        }

        // GET: api/users/{userId}
        // 通过用户ID获取其信息
        [Authorize]
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(string userId)
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
                return Ok(user);
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "查询失败" + ex.Message
                };
                //返回404
                return NotFound(errorResponse);
            }
        }

        // 更新用户资料
        [Authorize]
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] User updateUser)
        {
            if (userId != updateUser.UserId)
            {
                return BadRequest(new { msg = "用户ID不匹配" });
            }

            await _usersService.UpdateUserAsync(updateUser);
            return Ok(new { msg = "用户资料更新成功" });
        }

        // 更新用户状态
        [Authorize]
        [HttpPut("{userId}/updateStatus")]
        public async Task<IActionResult> UpdateUserStatus(string userId, [FromQuery] string newStatus)
        {
            try
            {
                var user = await _usersService.UpdateUserStatus(userId, newStatus);
               
                if (user == null)
                {
                    return NotFound(new { msg = "用户不存在" });
                }
                return Ok(new { 
                    msg = "用户状态已更新",
                    userId = user.UserId,
                    userStatus = user.UserStatus
                });
                //else
                //{
                //   return Ok(new { msg = "用户状态已更新", user });
                //}
            }
            catch (Exception ex)
            {
                var errorResponse = new
                {
                    msg = "更新用户状态失败: " + ex.Message
                };
                return StatusCode(500, errorResponse);
            }
        }
    }
}

