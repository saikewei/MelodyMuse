/*
与管理员管理用户有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using Microsoft.EntityFrameworkCore;
using MelodyMuse.Server.Services.Interfaces;

//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UsersController: Controller
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        // GET: api/users/{user_id}
        // 通过用户ID获取其信息
        [HttpGet]
        [HttpGet("{userId}")]
        public async Task<ActionResult<UsersModel>> GetUser(string userId)
        {
            var user = await _usersRepository.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }
}
