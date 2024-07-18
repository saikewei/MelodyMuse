/*
   用户服务(UserService)提供的接口(IUserService)
 */
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Services.Interfaces
{
    public interface IUsersService
    {
        Task<UserModel> GetUserById(string userId);
    }
}
