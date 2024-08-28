using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.models;

public partial class Songlist
{
    public string SonglistId { get; set; } = null!;

    public string? SonglistName { get; set; }

    public DateTime? SonglistDate { get; set; }

    public string? SonglistIspublic { get; set; }

    public string? UserId { get; set; }

    public virtual User? User { get; set; }

    public virtual ICollection<UserCollectSonglist> UserCollectSonglists { get; set; } = new List<UserCollectSonglist>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
