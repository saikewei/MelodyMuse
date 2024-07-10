using MelodyMuse.Server.models;
using MelodyMuse.Server.Models;
using MelodyMuse.Server.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MelodyMuse.Server.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ModelContext _context;

        public AccountRepository()
        {
            _context = new ModelContext();
        }

        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == loginModel.Username && u.Password == loginModel.Password);
                return user != null;
            }
            catch (Exception ex)
            {
                Console.WriteLine("发生异常：" + ex.Message);
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            //注册功能暂不能使用，需要修改
            try
            {
                // 查询当前最大的 UserId
                var maxUserId = await _context.Users.MaxAsync(u => u.UserId);

                // 生成下一个 UserId，UserId的格式为001,002......系统自动向后排序生成
                int nextUserIdNumber = maxUserId == null ? 1 : int.Parse(maxUserId.Substring(3)) + 1;
                string nextUserId = $"USR{nextUserIdNumber:D3}";

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
    }
}
