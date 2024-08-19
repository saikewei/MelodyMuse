/*
   UsersRepository提供的服务接口
 */
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;


namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IUsersRepository
    {
        Task<UserModel> GetUserById(string userId);
        Task<User?> UpdateUserStatus(string userId, string newStatus);
        Task<List<User>> GetAllUsers();
        
    }
}
