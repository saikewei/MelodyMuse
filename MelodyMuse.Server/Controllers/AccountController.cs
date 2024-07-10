/*
 * 与账户登陆注册 信息修改有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;


//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]
    //为当前控制器指定基础路由
    [Route("api/account")]

    //新建Account控制器类,继承于基控制器类
    public class AccountController:ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly IAccountService _accountService;

        //构造函数:初始化(传入相应的服务)接口
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //注册api/account/login路由
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)//接收到的数据自动绑定到loginModel数据块上
        {
            //调用下层服务接口提供的函数完成相应的逻辑操作
            var result = await _accountService.LoginAsync(loginModel);

            if (result)
            {
                return Ok("Login Successful");
            }

            return Unauthorized("Invaild Username or Password");
        }


        //注册api/account/register路由
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)//接收到的数据自动绑定到loginModel数据块上
        {
            //调用下层服务接口提供的函数完成相应的逻辑操作
            var result = await _accountService.RegisterAsync(registerModel);

            if (result)
            {
                return Ok("Register Successful");
            }

            return StatusCode(500, "Registration Failed");

        }

    }
}
