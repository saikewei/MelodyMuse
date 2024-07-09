using System.Threading.Tasks;
using MelodyMuse.Server.models;

namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IAccountService
    {
        Task<bool> LoginAsync(LoginModel loginModel);
        Task<bool> RegisterAsync(RegisterModel registerModel);
    }
}
