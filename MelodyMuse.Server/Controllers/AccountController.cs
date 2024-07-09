using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    [Route("api/account")]
    public class AccountController:ControllerBase
    {

        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }



        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var result = await _accountService.LoginAsync(loginModel);

            if (result)
            {
                return Ok("Login Successful");
            }

            return Unauthorized("Invaild Username or Password");
        }



        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel registerModel)
        {
            var result = await _accountService.RegisterAsync(registerModel);

            if (result)
            {
                return Ok("Register Successful");
            }

            return StatusCode(500, "Registration Failed");

        }

    }
}
