using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using Dapper;



namespace MelodyMuse.Server.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private readonly string _connectionString;

        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }



        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            try
            {using (var connection = new OracleConnection(_connectionString))
                {
                    var sql = "SELECT COUNT(1) FROM Users WHERE User_name = :Username AND Password = :Password";
                    var result = await connection.ExecuteScalarAsync<int>(sql, new { loginModel.Username, loginModel.Password });
                    return result > 0;
                }
            }
            catch (OracleException ex) when (ex.Number == 1005)
            {
                // 处理 ORA-01005 错误，例如提供友好的错误信息给用户
                Console.WriteLine("Oracle登录失败：密码错误");
                return false;
            }
            catch (OracleException ex)
            {
                // 其他 Oracle 异常处理
                Console.WriteLine("Oracle异常：" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // 其他异常处理
                Console.WriteLine("发生异常：" + ex.Message);
                return false;
            }
        }

        public async Task<bool> RegisterAsync(RegisterModel model)
        {
            try
            {using (var connection = new OracleConnection(_connectionString))
                {
                    var sql = "INSERT INTO Users (User_name, Password, PhoneNumber) VALUES (:Username, :Password, :PhoneNumber)";
                    var result = await connection.ExecuteAsync(sql, new { model.Username, model.Password, model.PhoneNumber });
                    return result > 0;
                }
            }
            catch (OracleException ex) when (ex.Number == 1005)
            {
                // 处理 ORA-01005 错误，例如提供友好的错误信息给用户
                Console.WriteLine("Oracle登录失败：密码错误");
                return false;
            }
            catch (OracleException ex)
            {
                // 其他 Oracle 异常处理
                Console.WriteLine("Oracle异常：" + ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                // 其他异常处理
                Console.WriteLine("发生异常：" + ex.Message);
                return false;
            }
        }
    }
}
