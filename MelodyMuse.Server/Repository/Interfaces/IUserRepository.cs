using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;

namespace MelodyMuse.Server.Repository.Interfaces
{
    public interface IUserRepository
    {
        // 获取用户信息
        Task<User> GetUserByIdAsync(string userId);

        // 更新用户资料
        Task UpdateUserAsync(User user);
    }
}
