/*
   UsersRepository提供的服务接口
 */
using MelodyMuse.Server.models;


namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserModel> GetUserById(string userId);
    }
}
