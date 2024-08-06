using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using Oracle.ManagedDataAccess.Client;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace MelodyMuse.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ModelContext _connectionString;

        public UserRepository()
        {
            _connectionString = new ModelContext();
        }
        // 获取用户信息
        public async Task<User> GetUserByIdAsync(string userId)
        {
            return await _connectionString.Users.FirstOrDefaultAsync(user => user.UserId == userId);
        }

 // 更新用户资料
        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _connectionString.Users.FindAsync(user.UserId);

            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Password = user.Password;
                existingUser.UserEmail = user.UserEmail;
                existingUser.UserPhone = user.UserPhone;
                existingUser.UserSex = user.UserSex;
                existingUser.UserAge = user.UserAge;
                existingUser.UserBirthday = user.UserBirthday;
                existingUser.UserStatus = user.UserStatus;

                await _connectionString.SaveChangesAsync();
            }
        }
    }
}
