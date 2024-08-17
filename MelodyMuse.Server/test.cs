using Oracle.ManagedDataAccess.Client;
using System.Xml.Linq;

namespace MelodyMuse.Server
{
    //用于测试后端和数据库的连接，Test函数可任意修改为想要测试的内容
    public static class test
    {
        public static string Test()  //测试函数示例，只能读一行，此处需要完善
        {
            using (OracleConnection connection = new OracleConnection(ServerConnectionConstants.connectionString))
            {
                // 打开数据库连接
                connection.Open();
                Console.WriteLine("Connected to Oracle Database");
                // 定义要执行的SQL查询语句
                string sql = "select * from C##MAIN_SCHEMA.ARTIST";
                // 创建一个OracleCommand对象，表示要在数据库上执行的SQL命令
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    // 执行查询，并返回一个OracleDataReader对象，用于读取查询结果
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        // 循环读取每一行结果
                        while (reader.Read())
                        {
                            // 获取名为 "ARTIST_ID" 列的值并转换为字符串
                            string id = reader.GetString(reader.GetOrdinal("ARTIST_ID"));
                            // 获取名为 "ARTIST_NAME" 列的值并转换为字符串
                            string name = reader.GetString(reader.GetOrdinal("ARTIST_NAME"));
                            // 返回读取到的结果，格式为 "ID{id}NAME{name}"
                            return "ID" + id + "NAME" + name;
                        }
                    }
                }
            }
            // 如果没有读取到任何行，返回一个空字符串
            return "";
        }
    }
}
