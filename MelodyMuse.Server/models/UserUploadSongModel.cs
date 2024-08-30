namespace MelodyMuse.Server.models
{
 

    public class SongUploadByUserModel
    {
        public string SongName { get; set; }
        public decimal Duration { get; set; }
        public string SongGenre { get; set; }
        public string Lyrics { get; set; }
        public IFormFile SongFile { get; set; }
    }
}
