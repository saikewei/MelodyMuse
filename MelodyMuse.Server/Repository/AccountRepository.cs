using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Text;

namespace MelodyMuse.Server.Repository
{
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

                if (user == null) {
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
            try
            {
                string nextUserId;
                // 获取当前时间的字符串表示
                string currentTimeString = DateTime.UtcNow.ToString("o"); // 使用ISO 8601格式

                // 计算哈希值
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

                    // 截取前10位作为用户ID
                    nextUserId = hashString.Substring(0, 10);
                }
               

                // 创建新用户对象
                var user = new User
                {
                    UserId = nextUserId,
                    UserName = model.Username,
                    Password = model.Password,
                    UserEmail = model.UserEmail,
                    UserPhone = model.UserPhone,
                    UserSex = model.UserSex,
                    UserAge = model.UserAge,
                    UserBirthday = model.UserBirthday,
                    UserStatus = model.UserStatus
                };

                // 添加用户到数据库
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常：" + ex.Message);
                return false;
            }
        }
      // 新方法：检查手机号是否已注册
        public async Task<User> GetUserByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.UserPhone == phoneNumber);
        }
        
    }
}
