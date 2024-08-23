namespace MelodyMuse.Server.models
{
public class SongPlayCountDto
{
    public string SongId { get; set; } = null!;
    public string? SongName { get; set; }
    public string? SongGenre { get; set; }
    public decimal? Duration { get; set; }
    public string? Lyrics { get; set; }
    public DateTime? SongDate { get; set; }
    public string? ComposerId { get; set; }
    public byte? Status { get; set; }
    public decimal TotalPlays { get; set; }
}


}
