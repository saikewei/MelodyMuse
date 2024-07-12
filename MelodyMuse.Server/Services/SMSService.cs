using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;

namespace MelodyMuse.Server.Services
{
    public class SMSService : ISMSService
    {
        //将缓存服务注入短信验证服务
        private readonly IVerificationCodeCacheService _verificationCodeCacheService;
        public SMSService(IVerificationCodeCacheService verificationCodeCacheService)
        {
            _verificationCodeCacheService = verificationCodeCacheService;   
        }


        public async Task<bool> SendSMSAsync(SendSMSModel _sendSMSModel)
        {
            string _phonenumber = _sendSMSModel.PhoneNumber;
            string _event = _sendSMSModel.Event;
            int _minutes = SMSConfigure.VerificationCodeValidity;
            TimeSpan _expiry = TimeSpan.FromMinutes(_minutes);

            try
            {
                //生成验证码
                string verificationCode = GenerateVerificationCode();
                //发送短信
                var result = true;
                //成功则添加进入缓存等待验证
                if (result)
                {
                    _verificationCodeCacheService.AddItemToCache(_phonenumber+_event, verificationCode,_expiry);
                    Console.WriteLine(verificationCode);
                    return true;
                }
                //失败则返回错误
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常:" + ex.Message);
                return false;
            }
        }

        public async Task<bool> VerifyCodeAsync(VerifyCodeModel _verifyCodeModel)
        {
            string _phonenumber = _verifyCodeModel.PhoneNumber;
            string _event = _verifyCodeModel.Event;
            string _verificationCode = _verifyCodeModel.VerificationCode;

            try
            {
                //根据model中的数据去验证池查看是否正确
                object result = _verificationCodeCacheService.GetItemFromCache(_phonenumber+_event);
                //没有相应信息
                if (result == null)
                {
                    return false;
                }
                string _trueVerificationCode = (string)result;

                //验证码正确
                if (_trueVerificationCode == _verificationCode)
                {
                    _verificationCodeCacheService.RemoveItemFromCache(_phonenumber+_event);
                    return true;
                }
                //错误
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常:" + ex.Message);
                return false;
            }
        }



        //生成验证码
        private string GenerateVerificationCode()
        {
            // 生成一个6位数字验证码
            Random random = new Random();
            int code = random.Next(100110, 998099);
            return code.ToString();
        }
    }
}
