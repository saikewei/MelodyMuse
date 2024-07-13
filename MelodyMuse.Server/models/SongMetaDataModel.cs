namespace MelodyMuse.Server.models
{
    public class SongMetaDataModel
    {
        public string? song_name {  get; set; } = string.Empty;
        public string song_id { get; set; } = string.Empty;
        
        public decimal? song_duration { get; set; }
        public DateTime? song_date { get; set; }
        public string? song_genre { get; set; }
        public string? composer_name { get; set; }
        public List<string?>? singer_names { get; set; }

        public string? song_url { get; set; }
    }
}
