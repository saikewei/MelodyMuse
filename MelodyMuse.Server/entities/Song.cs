using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class Song
{
    public string SongId { get; set; } = null!;

    public string? SongName { get; set; }

    public string? SongGenre { get; set; }

    public decimal? Duration { get; set; }

    public string? Lyrics { get; set; }

    public DateTime? SongDate { get; set; }

    public string? ArtistId { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<SongPlayCount> SongPlayCounts { get; set; } = new List<SongPlayCount>();

    public virtual ICollection<UploadCreatorSong> UploadCreatorSongs { get; set; } = new List<UploadCreatorSong>();

    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();

    public virtual ICollection<UserCollectSong> UserCollectSongs { get; set; } = new List<UserCollectSong>();

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Songlist> Songlists { get; set; } = new List<Songlist>();
}
