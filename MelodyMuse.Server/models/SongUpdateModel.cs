namespace MelodyMuse.Server.models
{
    public class SongUpdateModel
    {
        public string SongName { get; set; }
        public string SongGenre { get; set; }
        public string Lyrics { get; set; }
        public DateTime? SongDate { get; set; }
    }
}
