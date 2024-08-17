namespace MelodyMuse.Server.models
{
// 请求模型
    public class FollowArtistRequest
    {
        public string UserId { get; set; } = null!;
        public string ArtistId { get; set; } = null!;
    }
}
