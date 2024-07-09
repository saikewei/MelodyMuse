using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class UserCollectSong
{
    public string UserId { get; set; } = null!;

    public string SongId { get; set; } = null!;

    public DateTime? CollectSongDate { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
