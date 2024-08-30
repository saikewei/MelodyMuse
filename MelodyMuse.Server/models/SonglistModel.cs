namespace MelodyMuse.Server.models
{
    public class SonglistModel
    {
        public string SonglistId { get; set; }
        public string SonglistName { get; set; }
        public int SongCount { get; set; }
    }

    public class SonglistInfoModel
    {
        public string SonglistId { get; set; }
        public string SonglistName { get; set; }
        public DateTime? SonglistDate { get; set; }
        public string SonglistIsPublic { get; set; }
        public string UserId { get; set; }
        public string Username { get; set; }
    }
    public class Songlist_SongModel
    {
        public string SongId { get; set; }
        public string Title { get; set; }
        public decimal? Duration { get; set; }
        public List<Songlist_ArtistModel> Artists { get; set; }
    }

    public class Songlist_ArtistModel
    {
        public string ArtistId { get; set; }
        public string ArtistName { get; set; }
    }
}
