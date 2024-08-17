using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Services.Interfaces;
using System.Threading.Tasks;

namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // 获取用户信息
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);
            if (user != null)
            {
                return Ok(user);
            }

            return NotFound(new { msg = "用户未找到" });
        }

        // 更新用户资料
        [HttpPut("{userId}")]
        public async Task<IActionResult> UpdateUser(string userId, [FromBody] User updateUser)
        {
            if (userId != updateUser.UserId)
            {
                return BadRequest(new { msg = "用户ID不匹配" });
            }

            await _userService.UpdateUserAsync(updateUser);
            return Ok(new { msg = "用户资料更新成功" });
        }
    }
}
