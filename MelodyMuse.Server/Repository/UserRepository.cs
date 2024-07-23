using System.Collections.Generic;
using System.Threading.Tasks;
using MelodyMuse.Server.Models;
using Oracle.ManagedDataAccess.Client;
using MelodyMuse.Server.Repository.Interfaces;

namespace MelodyMuse.Server.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        // 获取用户信息
        public async Task<User> GetUserByIdAsync(string userId)
        {
            User user = null;

            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = "SELECT * FROM Users WHERE UserId = :userId";
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("userId", userId));

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            user = new User
                            {
                                UserId = reader["UserId"].ToString(),
                                UserName = reader["UserName"] as string,
                                Password = reader["Password"] as string,
                                UserEmail = reader["UserEmail"] as string,
                                UserPhone = reader["UserPhone"] as string,
                                UserSex = reader["UserSex"] as string,
                                UserAge = reader["UserAge"] as decimal?,
                                UserBirthday = reader["UserBirthday"] as DateTime?,
                                UserStatus = reader["UserStatus"] as string
                            };
                        }
                    }
                }
            }

            return user;
        }

        // 更新用户资料
        public async Task UpdateUserAsync(User user)
        {
            using (var connection = new OracleConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"
                    UPDATE Users
                    SET UserName = :userName,
                        Password = :password,
                        UserEmail = :userEmail,
                        UserPhone = :userPhone,
                        UserSex = :userSex,
                        UserAge = :userAge,
                        UserBirthday = :userBirthday,
                        UserStatus = :userStatus
                    WHERE UserId = :userId";
                
                using (var command = new OracleCommand(query, connection))
                {
                    command.Parameters.Add(new OracleParameter("userName", user.UserName));
                    command.Parameters.Add(new OracleParameter("password", user.Password));
                    command.Parameters.Add(new OracleParameter("userEmail", user.UserEmail));
                    command.Parameters.Add(new OracleParameter("userPhone", user.UserPhone));
                    command.Parameters.Add(new OracleParameter("userSex", user.UserSex));
                    command.Parameters.Add(new OracleParameter("userAge", user.UserAge));
                    command.Parameters.Add(new OracleParameter("userBirthday", user.UserBirthday));
                    command.Parameters.Add(new OracleParameter("userStatus", user.UserStatus));
                    command.Parameters.Add(new OracleParameter("userId", user.UserId));

                    await command.ExecuteNonQueryAsync();
                }
            }
        }
    }
}
