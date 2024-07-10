/*
 * Account相关操作Login Regeter用到的数据块
 */


namespace MelodyMuse.Server.models
{
    //定义登录所需数据块，包含用户名 密码
    public class LoginModel
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    public class RegisterModel
    {
        //注册时可以为空的就初始化为null，不可为空的初始化为""，UserStatus初始化为"1"
        public string UserId { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string? UserEmail { get; set; } = null;
        public string? UserPhone { get; set; } = null;
        public string? UserSex { get; set; } = null;
        public decimal? UserAge { get; set; } = null;
        public DateTime? UserBirthday { get; set; } = null;
        public string UserStatus { get; set; } = "1";

    }
}
