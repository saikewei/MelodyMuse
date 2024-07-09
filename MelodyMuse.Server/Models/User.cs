using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class User
{
    public string UserId { get; set; } = null!;

    public string? UserName { get; set; }

    public string? Password { get; set; }

    public string? UserEmail { get; set; }

    public string? UserPhone { get; set; }

    public string? UserSex { get; set; }

    public decimal? UserAge { get; set; }

    public DateTime? UserBirthday { get; set; }

    public string? UserStatus { get; set; }

    public virtual ICollection<Playlist> Playlists { get; set; } = new List<Playlist>();

    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    public virtual ICollection<SongPlayCount> SongPlayCounts { get; set; } = new List<SongPlayCount>();

    public virtual ICollection<Songlist> Songlists { get; set; } = new List<Songlist>();

    public virtual ICollection<UploadCreatorSong> UploadCreatorSongs { get; set; } = new List<UploadCreatorSong>();

    public virtual ICollection<Upload> Uploads { get; set; } = new List<Upload>();

    public virtual ICollection<UserCollectAlbum> UserCollectAlbums { get; set; } = new List<UserCollectAlbum>();

    public virtual ICollection<UserCollectSonglist> UserCollectSonglists { get; set; } = new List<UserCollectSonglist>();

    public virtual ICollection<UserCollectSong> UserCollectSongs { get; set; } = new List<UserCollectSong>();

    public virtual ICollection<Artist> Artists { get; set; } = new List<Artist>();
}
