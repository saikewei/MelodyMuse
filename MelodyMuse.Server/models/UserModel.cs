namespace MelodyMuse.Server.models
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
    public class AddUserCollectSongDto
{
    public string UserId { get; set; }
    public string SongId { get; set; }
}
public class AddUserCollectAlbumDto
    {
        public string UserId { get; set; } = null!;
        public string AlbumId { get; set; } = null!;
    }
public class UserCollectedSongDto
{
    public string SongId { get; set; }
    public string SongName { get; set; }
    public decimal Duration { get; set; }  // 修改为 decimal 以匹配 Song 实体中的类型
    public string ArtistName { get; set; }
}


}
