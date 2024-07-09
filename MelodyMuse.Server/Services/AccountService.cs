using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;



namespace MelodyMuse.Server.Services
{
    public class AccountService:IAccountService
    {
        private readonly IAccountRepository  _accountRepository;

        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        public async Task<bool> LoginAsync(LoginModel _loginModel)
        {
            return await _accountRepository.LoginAsync(_loginModel);
        }


        public async Task<bool> RegisterAsync(RegisterModel _registerModel)
        {
            return await (_accountRepository.RegisterAsync(_registerModel));
        }
    }
}
