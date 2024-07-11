using MelodyMuse.Server.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;



//Cache服务
namespace MelodyMuse.Server.Services
{
    //验证码专用Cache服务
    public class VerificationCodeCacheService: IVerificationCodeCacheService
    {
        private readonly IMemoryCache _memoryCache;

        //构造函数，初始化Cache以及验证码缓存时间(即有效时间)
        public VerificationCodeCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        //添加缓存项,参数分别为 索引 存储项  过期时间
        public void AddItemToCache(string key,object value, TimeSpan expiry)
        {
            _memoryCache.Set(key,value,expiry);
        }

        //获取缓存项
        public object GetItemFromCache(string key)
        {
            return _memoryCache.Get(key);
        }

        //移除缓存项
        public void RemoveItemFromCache(string key)
        {
            _memoryCache.Remove(key);   
        }
    }
}
