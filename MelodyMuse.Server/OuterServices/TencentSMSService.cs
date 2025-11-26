/*
    实现发送短信
 */


using MelodyMuse.Server.Configure;
using MelodyMuse.Server.models;
using MelodyMuse.Server.OuterServices.Interfaces;
using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Cvm.V20170312;
using TencentCloud.Cvm.V20170312.Models;
using TencentCloud.Ms.V20180408;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;
using static MelodyMuse.Server.OuterServices.Interfaces.ISmsSender;

namespace MelodyMuse.Server.OuterServices
{
    /// 腾讯云短信适配器
    /// 作用：将通用的 ISmsSender 请求转换为腾讯云 SDK 特定的 SendSmsRequest
    public class TencentSmsAdapter : ISmsSender
    {
        public TencentSmsAdapter()
        {
        }

        public async Task<bool> SendAsync(GenericSmsMessage message)
        {
            try
            {
                // 1. 初始化凭证
                Credential cred = new Credential
                {
                    SecretId = TencentSMSServiceConfigure.SecretId,
                    SecretKey = TencentSMSServiceConfigure.SecretKey,
                };

                // 2. 初始化客户端 (地域设为广州，可根据配置调整)
                SmsClient client = new SmsClient(cred, "ap-guangzhou");

                // 3. 组装腾讯云特定的 Request 对象 (适配过程)
                SendSmsRequest req = new SendSmsRequest
                {
                    SmsSdkAppid = TencentSMSServiceConfigure.SDKAppID,
                    Sign = TencentSMSServiceConfigure.SignName,
                    TemplateID = TencentSMSServiceConfigure.TemplateID,

                    // 适配：处理手机号格式 (添加 +86)
                    PhoneNumberSet = new string[] { "+86" + message.PhoneNumber },

                    // 适配：直接映射参数数组
                    TemplateParamSet = message.TemplateParams
                };

                // 4. 调用 SDK
                SendSmsResponse resp = client.SendSmsSync(req);

                // 5. 处理响应结果并转换为通用的 bool
                if (resp.SendStatusSet != null && resp.SendStatusSet.All(status => status.Code == "Ok"))
                {
                    return true;
                }
                else
                {
                    // 记录错误详情
                    if (resp.SendStatusSet != null)
                    {
                        foreach (var status in resp.SendStatusSet)
                        {
                            Console.WriteLine($"[TencentSMS Error] Phone: {status.PhoneNumber}, Code: {status.Code}, Msg: {status.Message}");
                        }
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("[TencentSMS Exception] " + ex.ToString());
                return false;
            }
        }
    }
}
