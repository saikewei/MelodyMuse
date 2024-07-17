/*
   UsersRepository提供的服务接口
 */
using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<User> GetUserByIdAsync(string userId);
    }
}
