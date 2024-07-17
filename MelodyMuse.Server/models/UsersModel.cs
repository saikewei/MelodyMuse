/*
 * Users相关操作，管理员管理界面所需
*/

namespace MelodyMuse.Server.models
{
    //一个用户所有的数据
    public class UsersModel
    {
        public string UserId { get; set; } = null!;
        public string UserName { get; set; } = null!;
        public string? Password { get; set; } = null;
        public string? UserEmail { get; set; } = null;
        public string? UserPhone { get; set; } = null;
        public string? UserSex { get; set; } = null;
        public decimal? UserAge { get; set; } = null;
        public DateTime? UserBirthday { get; set; } = null;
        public string UserStatus { get; set; } = "1";
    }
}
