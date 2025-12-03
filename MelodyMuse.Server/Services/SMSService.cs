using MelodyMuse.Server.models;
using MelodyMuse.Server.Services.Interfaces;
using MelodyMuse.Server.Configure;
using MelodyMuse.Server.OuterServices.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class SMSService : ISMSService
    {
        private readonly IVerificationCodeCacheService _verificationCodeCacheService;

        // 关键点：依赖抽象接口，而不是具体实现类
        private readonly ISmsSender _smsSender;

        public SMSService(IVerificationCodeCacheService verificationCodeCacheService, ISmsSender smsSender)
        {
            _verificationCodeCacheService = verificationCodeCacheService;
            _smsSender = smsSender;
        }

        public async Task<bool> SendSMSAsync(SendSMSModel _sendSMSModel)
        {
            // Refactored with adaptor Pattern
            string _phonenumber = _sendSMSModel.PhoneNumber;
            string _event = _sendSMSModel.Event;
            int _minutes = SMSConfigure.VerificationCodeValidity;
            TimeSpan _expiry = TimeSpan.FromMinutes(_minutes);

            try
            {
                // 1. 生成验证码
                string verificationCode = GenerateVerificationCode();

                // 2. 准备通用发送数据
                // 根据业务逻辑，按照约定顺序放入参数：[验证码, 事件名, 有效期]
                // 这样无论底层是用腾讯还是阿里，只要模板参数顺序一致即可兼容
                GenericSmsMessage smsMessage = new GenericSmsMessage
                {
                    PhoneNumber = _phonenumber,
                    TemplateParams = new string[]
                    {
                        verificationCode,
                        _event,
                        _minutes.ToString()
                    }
                };

                // 3. 调用通用接口发送短信
                bool result = await _smsSender.SendAsync(smsMessage);

                // 4. 成功则添加进入缓存等待验证
                if (result)
                {
                    // 将 Key 格式化为 "手机号+事件"
                    _verificationCodeCacheService.AddItemToCache(_phonenumber + _event, verificationCode, _expiry);
                    Console.WriteLine($"[Debug] Code Generated: {verificationCode}");
                    return true;
                }

                // 失败
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
                // 根据model中的数据去验证池查看是否正确
                object result = _verificationCodeCacheService.GetItemFromCache(_phonenumber + _event);

                // 没有相应信息
                if (result == null)
                {
                    return false;
                }

                string _trueVerificationCode = (string)result;

                // 验证码正确
                if (_trueVerificationCode == _verificationCode)
                {
                    _verificationCodeCacheService.RemoveItemFromCache(_phonenumber + _event);
                    return true;
                }

                // 错误
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常:" + ex.Message);
                return false;
            }
        }

        // 生成验证码
        private string GenerateVerificationCode()
        {
            Random random = new Random();
            int code = random.Next(100110, 998099);
            return code.ToString();
        }
    }
}
