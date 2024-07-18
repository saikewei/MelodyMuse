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

        public UsersService(IUsersRepository usersRepository)
        {
            _usersRepository = usersRepository;
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
    }
}
