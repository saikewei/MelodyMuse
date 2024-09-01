/*
   用户服务(UserSrvice)的函数实现(调用下一层repository提供的接口)
 */
using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;


namespace MelodyMuse.Server.Services
{
    public class UsersService: IUsersService
    {
        //内部维护一个下层数据库访问服务(Repository)的接口
        private readonly IUsersRepository _usersRepository;
        private readonly IArtistRepository _artistRepository;

        public UsersService(IUsersRepository usersRepository, IArtistRepository artistRepository)
        {
            _usersRepository = usersRepository;
            _artistRepository = artistRepository;
        }
        // 构造函数，注入实例
        public UsersService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
        }

        // 根据用户ID获取用户元数据
        public async Task<UserModel> GetUserById(string userId)
        {
            // 调用下层接口，从仓库层获取用户信息
            return await _usersRepository.GetUserById(userId);
        }

        //更新用户数据
        public async Task<User?> UpdateUserStatus(string userId, string newStatus)
        {
            return await _usersRepository.UpdateUserStatus(userId, newStatus);
        }

        //获取用户列表
        public async Task<List<string>> GetAllUserIds()
        {
            var users = await _usersRepository.GetAllUsers();
            return users.Select(u => u.UserId).ToList();
        }

        // 更新用户资料
        public async Task UpdateUserAsync(User user)
        {
            // 调用 IUserRepository 中的方法更新用户资料
            await _usersRepository.UpdateUserAsync(user);
        }

        public async Task<bool> Useridentity(string userId)
        {
            return await _artistRepository.IsUserInArtistAsync(userId);
        }
    }
}
