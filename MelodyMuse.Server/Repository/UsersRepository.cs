using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace MelodyMuse.Server.Repository
{
    // 用户仓库类，实现IUsersRepository接口
    public class UsersRepository : IUsersRepository
    {
        private readonly ModelContext _context;

        // 构造函数，创建新的ModelContext对象
        public UsersRepository()
        {
            _context = new ModelContext();
        }

        // 根据用户ID获取用户信息
        public async Task<UserModel> GetUserById(string userId)
        {
            // 在数据库中查找是否有该用户ID，返回该用户的数据
            var userData = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);

            if (userData == null)
            {
                return null;
            }
            // 创建新用户对象
            var user = new UserModel
            {
                UserId = userData.UserId,
                UserName = userData.UserName,
                Password = userData.Password,
                UserEmail = userData.UserEmail,
                UserPhone = userData.UserPhone,
                UserSex = userData.UserSex,
                UserAge = userData.UserAge,
                UserBirthday = userData.UserBirthday,
                UserStatus = userData.UserStatus
            };

            return user;
        }
    }
}
