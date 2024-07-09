namespace MelodyMuse.Server
{
    //用于建立和数据库连接的类，使用connectionString即可
    public static class ServerConnectionConstants
    {
        public const string datasource = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=101.126.23.58)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=orcl)));";
        public const string userId = "User Id=SYS";
        public const string password = "Password=tongjiorcl2024";
        public const string DBAPrivilege = "DBA Privilege=SYSDBA";
        public const string connectionString = userId + password + datasource;
    }
}

