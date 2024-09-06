﻿/*
  账户服务(AccountSrvice)的函数实现(调用下一层repository提供的接口)
 */



using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using MelodyMuse.Server.Services.Interfaces;
using System.Security.Cryptography;
using System.Text;



namespace MelodyMuse.Server.Services
{
    //定义账户服务(AccountService)类，继承账户服务接口(IAccountService)
    //完成接口内部函数实现
    public class AccountService : IAccountService
    {
        //内部维护一个下层数据库访问服务(Repository)的接口
        private readonly IAccountRepository _accountRepository;

        //传入服务接口
        public AccountService(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }


        //登录:调用下层接口提供的登陆验证服务
        public async Task<GenerateTokenModel> LoginAsync(LoginModel _loginModel)
        {
            //替换哈希值
            _loginModel.Password = caculateSha256Hash(_loginModel.Password);
            return await _accountRepository.LoginAsync(_loginModel);
        }


        //注册:调用下层接口提供的注册服务
        public async Task<bool> RegisterAsync(RegisterModel _registerModel)
        {
            //替换哈希值
            _registerModel.Password =caculateSha256Hash(_registerModel.Password);

            return await _accountRepository.RegisterAsync(_registerModel);
        }
        public async Task<bool> CheckPhoneNumberExistsAsync(string phoneNumber) // 实现新方法
        {
            var user = await _accountRepository.GetUserByPhoneNumberAsync(phoneNumber);
            return user != null;
        }
        public async Task<bool> ResetPasswordAsync(string phoneNumber, string Password)
        {
            try
            {
                // 查找用户
                var user = await _accountRepository.GetUserByPhoneNumberAsync(phoneNumber);

                if (user == null)
                {
                    return false; // 用户不存在
                }
                // 更新用户密码
                user.Password = caculateSha256Hash(Password);

                // 保存更改
                await _accountRepository.UpdateUserAsync(user);

                return true;
            }
            catch (Exception ex)
            {
                // 记录异常
                Console.WriteLine("Exception in ResetPasswordAsync: " + ex.Message);
                return false;
            }
        }

        private string caculateSha256Hash(string rawData)
        {
            // 使用SHA256类计算哈希值
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // 将输入字符串转换为字节数组并计算哈希值
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // 将字节数组转换为十六进制字符串
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}
