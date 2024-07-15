/*
 * SMS相关操作发送短信  验证用到的数据块
 */


namespace MelodyMuse.Server.models
{
    public class SendSMSModel
    {   //接受要发送短信的号码
        public string? PhoneNumber { get; set; } = null;
        public string? Event { get; set; } = null;
    }


    //接受验证的手机号以及用户输入的验证码
    public class VerifyCodeModel
    {
        public string? PhoneNumber { get; set; } = null;
        public string? Event { get; set; } = null;
        public string? VerificationCode { get; set; } = null;
    }


    //接受
    public class SendToTencentModel
    {
        public string? PhoneNumber { get; set; } = null;
        public string? Event { get; set; } = null;
        public string? VerificationCode { get; set; } = null;
        public int? VerificationCodeValidityTime { get; set; } = null;
    }
}
