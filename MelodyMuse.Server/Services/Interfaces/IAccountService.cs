/*
   账户服务(AccountService)提供的接口(IAccountService)
 */
using MelodyMuse.Server.models;



namespace MelodyMuse.Server.Services.Interfaces
{
    //定义接口(一个不包含函数实现的函数列表)
    public interface IAccountService
    {
        Task<GenerateTokenModel> LoginAsync(LoginModel loginModel);
        Task<bool> RegisterAsync(RegisterModel registerModel);
        
    }
}
