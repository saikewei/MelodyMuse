/*
 * 与账户登陆注册 信息修改有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.Services;


//命名空间:Controllers
namespace MelodyMuse.Server.Controllers
{
    //启用接收数据自动绑定[FromBody]
    [ApiController]
    //注册/api/account分支路由
    [Route("api/account")]
    public class AccountController : ControllerBase
    {
        //维护一个到下层服务的接口
        private readonly IAccountService _accountService;

        //构造函数:初始化(传入相应的服务)接口
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }


        //注册login路由
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)//接收到的数据自动绑定到loginModel数据块上
        {
            //调用下层服务接口提供的函数完成相应的逻辑操作
            GenerateTokenModel UserInfo = await _accountService.LoginAsync(loginModel);

            if (UserInfo!=null)
            {
                //根据用户信息生成JWT
                var JWT = JWTGenerator.GenerateToken(UserInfo,JWTConfigure.serect_key);
                var seccessResponse = new
                {
                    msg = "登录成功！",
                    Token = JWT
                };
                return Ok(seccessResponse);
            }

            var failResponse = new
            {
                msg = "用户名或密码错误！"
            };
            return Unauthorized(failResponse);
        }


        //注册register路由
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)//接收到的数据自动绑定到loginModel数据块上
        {
            //调用下层服务接口提供的函数完成相应的逻辑操作
            var result = await _accountService.RegisterAsync(registerModel);

            if (result)
            {
                var seccessResponse = new
                {
                    msg = "注册成功！"
                };
                return Ok(seccessResponse);
            }

            var failResponse = new
            {
                msg = "注册失败！"
            };
            return Unauthorized(failResponse);
        }
    }

}