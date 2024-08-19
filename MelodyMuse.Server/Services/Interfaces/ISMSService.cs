/*
   短信服务(SMSService)提供的接口(IAccountService)
 */
using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface ISMSService
    {
        Task<bool> SendSMSAsync(SendSMSModel sendSMSModel);
        Task<bool> VerifyCodeAsync(VerifyCodeModel verifyCodeModel);
    }
}
