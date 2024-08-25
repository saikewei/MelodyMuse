/*
 * Account相关操作Login Regeter用到的数据块
 */


namespace MelodyMuse.Server.models
{
    //定义登录所需数据块，包含用户名 密码
    public class LoginModel
    {
        public string PhoneNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }


    public class RegisterModel
    {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? UserEmail { get; set; } = null;
        public string? UserPhone { get; set; } = null;
        public string? UserSex { get; set; } = null;
        public decimal? UserAge { get; set; } = null;
        public DateTime? UserBirthday { get; set; } = null;
        public string UserStatus { get; set; } = "1";

    }
// 定义更新用户信息的数据块，所有属性都更新
    public class UpdateUserModel
    {
        public string? Username { get; set; } = null;
        public string? Password { get; set; } = null;
        public string? UserEmail { get; set; } = null;
        public string? UserPhone { get; set; } = null;
        public string? UserSex { get; set; } = null;
        public decimal? UserAge { get; set; } = null;
        public DateTime? UserBirthday { get; set; } = null;
        public string UserStatus { get; set; } = "1";

    }

    public class PhoneNumberModel
    {
        public string PhoneNumber { get; set; }
    }

    public class ChangePasswordModel
    {
        public string? phoneNumber { get; set; }
        public string? Password  { get; set; }
    }

}
