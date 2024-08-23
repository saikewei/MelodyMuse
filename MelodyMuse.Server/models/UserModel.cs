﻿namespace MelodyMuse.Server.models
{
    public class UserModel
    {
        public string UserId { get; set; } = null!;

        public string UserName { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? UserEmail { get; set; }

        public string? UserPhone { get; set; }

        public string? UserSex { get; set; }

        public decimal? UserAge { get; set; }

        public DateTime? UserBirthday { get; set; }

        public string? UserStatus { get; set; }
    }
}
