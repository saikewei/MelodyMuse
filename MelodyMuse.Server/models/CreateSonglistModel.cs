namespace MelodyMuse.Server.models
{
    public class CreateSonglistModel
    {
        public string SonglistId { get; set; }
        public string SonglistName { get; set; }
        public DateTime SonglistDate { get; set; }
        public string IsPublic { get; set; }
        public string UserId { get; set; }
    }
}
