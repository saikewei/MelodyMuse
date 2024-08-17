using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class Playlist
{
    public string UserId { get; set; } = null!;

    public string SongId { get; set; } = null!;

    public decimal? PlayOrder { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
