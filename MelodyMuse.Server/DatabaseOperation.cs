using Oracle.ManagedDataAccess.Client;

namespace MelodyMuse.Server
{
    public static class DatabaseOperation
    {
        public static List<Dictionary<string, object>> Select(string sql)
        {
            List<Dictionary<string, object>> result = new List<Dictionary<string, object>>();
            using (OracleConnection connection = new OracleConnection(ServerConnectionConstants.connectionString))
            {
                // 打开数据库连接
                connection.Open();
                Console.WriteLine("Connected to Oracle Database");

                // 创建一个OracleCommand对象，表示要在数据库上执行的SQL命令
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    // 执行查询，并返回一个OracleDataReader对象，用于读取查询结果
                    using (OracleDataReader reader = command.ExecuteReader())
                    {
                        // 循环读取每一行结果
                        while (reader.Read())
                        {
                            Dictionary<string, object> row = new Dictionary<string, object>();
                            // 遍历每一列
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                // 获取列名
                                string columnName = reader.GetName(i);

                                // 获取列值
                                object columnValue = reader.GetValue(i);

                                // 添加到字典中
                                row[columnName] = columnValue;
                            }
                            //把行存入结果
                            result.Add(row);
                        }
                    }
                }
            }

            // 返回读取到的结果字典列表
            return result;
        }

        public static int Insert(string sql, Dictionary<string, object> parameters)
        {
            // 使用指定的连接字符串创建并打开Oracle数据库连接
            using (OracleConnection connection = new OracleConnection(ServerConnectionConstants.connectionString))
            {
                connection.Open();  // 打开数据库连接
                Console.WriteLine("Connected to Oracle Database");  // 输出连接成功信息

                // 创建一个OracleCommand对象，表示要在数据库上执行的SQL命令
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    // 遍历参数字典，将每个参数添加到OracleCommand对象中
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(new OracleParameter(param.Key, param.Value));
                    }

                    // 执行插入操作，返回受影响的行数
                    return command.ExecuteNonQuery();
                }
            }
        }

        // 更新操作
        public static int Update(string sql, Dictionary<string, object> parameters)
        {
            // 使用指定的连接字符串创建并打开Oracle数据库连接
            using (OracleConnection connection = new OracleConnection(ServerConnectionConstants.connectionString))
            {
                connection.Open();  // 打开数据库连接
                Console.WriteLine("Connected to Oracle Database");  // 输出连接成功信息

                // 创建一个OracleCommand对象，表示要在数据库上执行的SQL命令
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    // 遍历参数字典，将每个参数添加到OracleCommand对象中
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(new OracleParameter(param.Key, param.Value));
                    }

                    // 执行更新操作，返回受影响的行数
                    return command.ExecuteNonQuery();
                }
            }
        }

        // 删除操作
        public static int Delete(string sql, Dictionary<string, object> parameters)
        {
            // 使用指定的连接字符串创建并打开Oracle数据库连接
            using (OracleConnection connection = new OracleConnection(ServerConnectionConstants.connectionString))
            {
                connection.Open();  // 打开数据库连接
                Console.WriteLine("Connected to Oracle Database");  // 输出连接成功信息

                // 创建一个OracleCommand对象，表示要在数据库上执行的SQL命令
                using (OracleCommand command = new OracleCommand(sql, connection))
                {
                    // 遍历参数字典，将每个参数添加到OracleCommand对象中
                    foreach (var param in parameters)
                    {
                        command.Parameters.Add(new OracleParameter(param.Key, param.Value));
                    }

                    // 执行删除操作，返回受影响的行数
                    return command.ExecuteNonQuery();
                }
            }
        }
    }
}
