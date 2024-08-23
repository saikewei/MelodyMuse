/*
    实现发送短信
 */


using MelodyMuse.Server.models;
using MelodyMuse.Server.OuterServices.Interfaces;
using MelodyMuse.Server.Configure;

using System;
using System.Threading.Tasks;
using TencentCloud.Common;
using TencentCloud.Cvm.V20170312;
using TencentCloud.Cvm.V20170312.Models;
using TencentCloud.Ms.V20180408;
using TencentCloud.Sms.V20190711;
using TencentCloud.Sms.V20190711.Models;

namespace MelodyMuse.Server.OuterServices
{
    public class TencentSMSService:ITencentSMSService
    {

        public TencentSMSService()
        {
        }


        public async Task<bool> SendSMSAsync(SendToTencentModel sendToTencentModel)
        {

            try
            {
                // 为了保护密钥安全，建议将密钥设置在环境变量中或者配置文件中。
                // 硬编码密钥到代码中有可能随代码泄露而暴露，有安全隐患，并不推荐。
                // 这里采用的是从环境变量读取的方式，需要在环境变量中先设置这两个值。
                Credential cred = new Credential
                {
                    SecretId = TencentSMSServiceConfigure.SecretId,
                    SecretKey = TencentSMSServiceConfigure.SecretKey,
                };

                SmsClient client = new SmsClient(cred, "ap-guangzhou");

                SendSmsRequest req = new SendSmsRequest
                {
                    SmsSdkAppid = TencentSMSServiceConfigure.SDKAppID,  // 短信SdkAppid
                    Sign = TencentSMSServiceConfigure.SignName,  // 短信签名
                    TemplateID = TencentSMSServiceConfigure.TemplateID,  // 短信模板ID
                    PhoneNumberSet = new string[] { "86"+sendToTencentModel.PhoneNumber },  // 接收短信的手机号
                    
                    TemplateParamSet = new string[] { sendToTencentModel.VerificationCode,
                        sendToTencentModel.Event,
                        sendToTencentModel.VerificationCodeValidityTime.ToString() }  // 模板参数
                    
                    /*TemplateParamSet = new string[] { sendToTencentModel.VerificationCode,
                        sendToTencentModel.VerificationCodeValidityTime.ToString() }  // 模板参数*/
                };

                SendSmsResponse resp = client.SendSmsSync(req);
                //Console.WriteLine(AbstractModel.ToJsonString(resp));


                // 判断短信是否发送成功
                //可以一组好几个手机号
                if (resp.SendStatusSet != null && resp.SendStatusSet.All(status => status.Code == "Ok"))
                {
                    return true;
                }
                else
                {
                    //所以Status不止一个
                    foreach (var status in resp.SendStatusSet)
                    {
                        Console.WriteLine($"PhoneNumber: {status.PhoneNumber}, Code: {status.Code}, Message: {status.Message}");
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("发生异常"+e);
                return false;
            }
        }
    }
}
