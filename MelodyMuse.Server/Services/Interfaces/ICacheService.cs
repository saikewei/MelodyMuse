

//缓存服务的接口
namespace MelodyMuse.Server.Services.Interfaces
{
    //短信验证码专用服务接口
    public interface IVerificationCodeCacheService
    {
        void AddItemToCache(string key, object value,TimeSpan expiry);
        object GetItemFromCache(string key);
        void RemoveItemFromCache(string key);

    }
}
