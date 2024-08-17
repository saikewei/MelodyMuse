/*
   Accountrepository提供的服务接口
 */


using System.Threading.Tasks;
using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IAccountRepository
    {
        Task<GenerateTokenModel> LoginAsync(LoginModel loginModel);
        Task<bool> RegisterAsync(RegisterModel registerModel);  
    }
}
