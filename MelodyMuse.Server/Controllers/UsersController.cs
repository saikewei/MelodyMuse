/*
与管理员管理用户有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.models;

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

        // GET: api/users/{userId}
        // 通过用户ID获取其信息
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

        // 更新用户状态
        [HttpPut("{userId}/{newStatus}")]
        public async Task<IActionResult> UpdateUserStatus(string userId, string newStatus)
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

