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
        public async Task<User> GetUserByIdAsync(string userId)
        {
            // 在数据库中查找是否有该用户ID
            return await _context.Users
                .FromSqlInterpolated($"SELECT * FROM Users WHERE USER_ID = {userId}")
                .FirstOrDefaultAsync();
        }
    }
}
