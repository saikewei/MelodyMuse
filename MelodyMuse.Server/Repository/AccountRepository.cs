using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;
using System.Collections.Generic;

namespace MelodyMuse.Server.Repository
{
    // 1. 策略接口
    public interface IUserIdGenerationStrategy
    {
        string GenerateId();
    }

    // 2. 具体策略：哈希策略 (现在的逻辑)
    public class HashTimeIdStrategy : IUserIdGenerationStrategy
    {
        public string GenerateId()
        {
            string currentTimeString = DateTime.UtcNow.ToString("o");
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(currentTimeString));

                // 将哈希值转换为16进制字符串
                StringBuilder hashStringBuilder = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    hashStringBuilder.Append(b.ToString("x2"));
                }

                string hashString = hashStringBuilder.ToString();

                return hashStringBuilder.ToString().Substring(0, 10);
            }
        }
    }

    // 3. 具体策略：GUID 策略 (备用)
    public class GuidIdStrategy : IUserIdGenerationStrategy
    {
        public string GenerateId() => Guid.NewGuid().ToString("N").Substring(0, 10);
    }

    public class AccountRepository : IAccountRepository
    {
        private readonly ModelContext _context;

        public AccountRepository()
        {
            _context = new ModelContext();
        }

        public async Task<GenerateTokenModel> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserPhone == loginModel.PhoneNumber && u.Password == loginModel.Password);

                if (user == null)
                {
                    return null;
                }

                var UserInfo = new GenerateTokenModel
                {
                    Username = user.UserName,
                    UserID = user.UserId,
                    UserPhone = user.UserPhone
                };

                return UserInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常：" + ex.Message);
                return null;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {

            if (model.UserPhone == null)
            {
                throw new Exception("手机号不可以为空");
            }

            if (model.Username == null)
            {
                throw new Exception("用户名不可以为空");
            }

            // 检查用户手机号是否已注册
            var checkUserByPhone = await _context.Users.CountAsync(u => u.UserPhone == model.UserPhone);
            if (checkUserByPhone != 0)
            {
                throw new Exception("手机号已被注册");
            }

            // 检查用户名是否已注册
            var checkUserByName = await _context.Users.CountAsync(u => u.UserName == model.Username);
            if (checkUserByName != 0)
            {
                throw new Exception("用户名已被注册");
            }

            IUserIdGenerationStrategy idStrategy = new HashTimeIdStrategy();
            string nextUserId = idStrategy.GenerateId();

            // 创建新用户对象
            var user = new User
            {
                UserId = nextUserId,
                UserName = model.Username,
                Password = model.Password,
                UserEmail = null,
                UserPhone = model.UserPhone,
                UserSex = null,
                UserAge = null,
                UserBirthday = null,
                UserStatus = null
            };

            // 添加用户到数据库
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return true;
        }
        // 新方法：检查手机号是否已注册
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserPhone == phoneNumber);
        }
        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
