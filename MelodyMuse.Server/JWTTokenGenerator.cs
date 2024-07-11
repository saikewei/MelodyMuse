using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace MelodyMuse.Server
{
    public class JWTTokenGenerator
    {
        public static string GenerateToken(string Phonenumber , string secretKey)
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
                    new Claim(ClaimTypes.MobilePhone,Phonenumber),
                }),
                //JWT过期时间
                Expires = DateTime.UtcNow.AddHours(1),
                //记录算法和密钥
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            //生成JWT   JWT是一种特殊的token
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //将JWT转换为字符串返回
            return tokenHandler.WriteToken(token);

        }
    }
}
