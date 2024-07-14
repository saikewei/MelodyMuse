namespace MelodyMuse.Server.Configure
{
    public class SMSConfigure
    {
        public const int VerificationCodeValidity = 5;   //验证码有效时间5分钟
    }

    public class JWTConfigure
    {
        public const string serect_key = "qwertyuiopasdfghjklzxcvbnm";    //JWT生成与解析的密钥
        public const int JsonWebTokenTValidity = 3;             //Token有效期,单位:小时
    }
}
