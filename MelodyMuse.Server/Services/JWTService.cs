using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

//[Authorize]  需要授权的路由

namespace MelodyMuse.Server.Services
{
    public class JWTGenerator
    {
        public static string GenerateToken(GenerateTokenModel generateTokenModel, string secretKey)
        {
            //创建新的JWTSecurityTokenHandler,用于创建和处理JWT
            var tokenHandler = new JwtSecurityTokenHandler();
            //将传入的 secretKey 字符串转换为字节数组，作为 HMACSHA256 签名算法的密钥。
            var key = Encoding.ASCII.GetBytes(secretKey);
            //新建标记描述符，存放header payload secret
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, generateTokenModel.Username),
                    new Claim(ClaimTypes.NameIdentifier,generateTokenModel.UserID),
                    new Claim(ClaimTypes.MobilePhone,generateTokenModel.UserPhone),

                }),
                //JWT过期时间
                Expires = DateTime.UtcNow.AddHours(JWTConfigure.JsonWebTokenTValidity),
                //记录算法和密钥
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //生成JWT   JWT是一种特殊的token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //将JWT转换为字符串返回
            return tokenHandler.WriteToken(token);

        }
    }



    public static class TokenParser
    {
        public static ParsedTokenModel ParseToken(string token, string secretKey)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
                var username = principal.FindFirst(ClaimTypes.Name)?.Value;
                var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                var phoneNumber = principal.FindFirst(ClaimTypes.MobilePhone)?.Value;

                return new ParsedTokenModel
                {
                    Username = username,
                    UserID = userId,
                    UserPhone = phoneNumber
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常"+ex);
                return null;
            }
        }
    }
}
