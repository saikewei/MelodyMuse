/*
 * 与验证短信发送验证有关的api注册与数据接收
*/
using Microsoft.AspNetCore.Mvc;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Services;
using MelodyMuse.Server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Security.Policy;
using MelodyMuse.Server.Configure;


namespace MelodyMuse.Server.Controllers
{

    //[Authorize]     //开启验证服务

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
            // 从请求头中获取 JWT 令牌
            //var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //如果没有令牌，返回未授权错误码401
            //if (token == null)
            //{
            //    return Unauthorized();
            //}
            // 解析 JWT 令牌 得到存储的信息 ParsedToken:id,name,phone
            //var parsedToken = TokenParser.ParseToken(token,JWTConfigure.serect_key);
            //下面是输出测试查看是否正确
            //Console.WriteLine(parsedToken.UserID+" "+parsedToken.Username+" "+parsedToken.UserPhone);




            var result = await _smsService.SendSMSAsync(sendsmsModel);

            if (result)
            {

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
