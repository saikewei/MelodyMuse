/*
  账户服务(AccountSrvice)的函数实现(调用下一层repository提供的接口)
 */



using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;



namespace MelodyMuse.Server.Services
{
    //定义账户服务(AccountService)类，继承账户服务接口(IAccountService)
    //完成接口内部函数实现
    public class AccountService:IAccountService
    {
        //内部维护一个下层数据库访问服务(Repository)的接口
        private readonly IAccountRepository  _accountRepository;

        //传入服务接口
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        //登录:调用下层接口提供的登陆验证服务
        public async Task<bool> LoginAsync(LoginModel _loginModel)
        {
            return await _accountRepository.LoginAsync(_loginModel);
        }


        //注册:调用下层接口提供的注册服务
        public async Task<bool> RegisterAsync(RegisterModel _registerModel)
        {
            return await (_accountRepository.RegisterAsync(_registerModel));
        }
    }
}
