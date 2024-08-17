using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;

namespace MelodyMuse.Server.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // 获取用户信息
        public async Task<User> GetUserByIdAsync(string userId)
        {
            // 调用 IUserRepository 中的方法获取用户信息
            return await _userRepository.GetUserByIdAsync(userId);
        }

        // 更新用户资料
        public async Task UpdateUserAsync(User user)
        {
            // 调用 IUserRepository 中的方法更新用户资料
            await _userRepository.UpdateUserAsync(user);
        }
    }
}
