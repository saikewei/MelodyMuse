using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.models;

public partial class Song
{
    public string SongId { get; set; } = null!;

    public string? SongName { get; set; }

    public string? SongGenre { get; set; }

    public decimal? Duration { get; set; }

    public string? Lyrics { get; set; }

    public DateTime? SongDate { get; set; }

    public string? ComposerId { get; set; }

    public byte? Status { get; set; }

    public virtual Artist? Composer { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<SongPlayCount> SongPlayCounts { get; set; } = new List<SongPlayCount>();

    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();

    public virtual ICollection<UserCollectSong> UserCollectSongs { get; set; } = new List<UserCollectSong>();

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();

    public virtual ICollection<Songlist> Songlists { get; set; } = new List<Songlist>();
}
