/*
 *  实际发送短信请求的接口
 */
using MelodyMuse.Server.models;


namespace MelodyMuse.Server.OuterServices.Interfaces
{
    //发短信的借口
    public interface ITencentSMSService
    {
        Task<bool> SendSMSAsync(SendToTencentModel sendToTencentModel);
    }
}
