namespace MelodyMuse.Server.Configure
{
    public class SMSConfigure
    {
        public const int VerificationCodeValidity = 5;   //验证码有效时间5分钟
    }

    public class TencentSMSServiceConfigure
    {
        public const string SecretId = "AKIDT2sjb5cVL8zsrzInSKRiFpR1HSWeigWI";
        public const string SecretKey = "oX8bxEUjIqNzjQJJKUpeaUMUlzJalihp";
        public const string SDKAppID = "1400923817";
        public const string TemplateID = "2215573";   //2215574   
        public const string SignName = "tongjiStore网";
    }

    public class JWTConfigure
    {
        public const string serect_key = "qwertyuiopasdfghjklzxcvbnm";    //JWT生成与解析的密钥
        public const int JsonWebTokenTValidity = 3;             //Token有效期,单位:小时
    }

    public class FtpSettings
    {
        public string Server { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
