/*
    AccountRepository的函数实现
 */


using MelodyMuse.Server.models;
using MelodyMuse.Server.Repository.Interfaces;
using Oracle.ManagedDataAccess.Client;
using System.Threading.Tasks;
using Dapper;



namespace MelodyMuse.Server.Repository
{

    //定义AccountRepository数据库服务实现类,继承IAccountRepository接口
    public class AccountRepository : IAccountRepository
    {
        //内部维护连接数据库的string字段
        private readonly string _connectionString;

        //构造函数，传入_connectionString,在program.cs中调用并传入_connectionString
        public AccountRepository(string connectionString)
        {
            _connectionString = connectionString;
        }


        //具体的数据库交互实现:
        public async Task<bool> LoginAsync(LoginModel loginModel)
        {
            try
            {
                using (var connection = new OracleConnection(_connectionString))
                {
                    connection.Open();
                    var sql = "SELECT COUNT(1) FROM Users WHERE User_name = :Username AND Password = :Password";
                    using (var command = new OracleCommand(sql, connection))
                    {
                        command.Parameters.Add(new OracleParameter("Username", loginModel.Username));
                        command.Parameters.Add(new OracleParameter("Password", loginModel.Password));

                        var result = Convert.ToInt32(command.ExecuteScalar());
                        return result > 0;
                    }
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
