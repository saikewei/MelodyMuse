/*
 * 与验证短信发送验证有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;


namespace MelodyMuse.Server.Controllers
{
    [ApiController]
    //短信服务路由注册
    [Route("api/sms")]


    public class SMSController:ControllerBase
    {
        private readonly ISMSService _smsService;

        public SMSController(ISMSService smsService)
        {
            _smsService = smsService;
        }



        //注册sendsms路由
        [HttpPost("sendsms")]
        public async Task<IActionResult> SendSMS([FromBody] SendSMSModel sendsmsModel)
        {
            var result = await _smsService.SendSMSAsync(sendsmsModel);

            if (result)
            {
                Console.WriteLine(User.FindFirst(ClaimTypes.MobilePhone)?.Value);
                var successResponse = new
                {
                    msg = "成功发送验证短信"
                };

                return Ok(successResponse);
            }

            var failResponse = new
            {
                msg = "验证短信发送失败"
            };

            return StatusCode(StatusCodes.Status502BadGateway, failResponse);

        }



        //注册verifycode路由
        [HttpPost("verifycode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeModel verifyCodeModel)
        {
            var result = await _smsService.VerifyCodeAsync(verifyCodeModel);

            if (result)
            {
                var trueresponse = new
                {
                    msg = "验证成功"
                };

                return Ok(trueresponse);
            }

            var falseResponse = new
            {
                msg = "验证码不匹配或者已过期"
            };

            return UnprocessableEntity(falseResponse);

        }
    }

    

}
