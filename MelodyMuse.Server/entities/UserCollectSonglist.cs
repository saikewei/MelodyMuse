using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.models;

public partial class UserCollectSonglist
{
    public string UserId { get; set; } = null!;

    public string SonglistId { get; set; } = null!;

    public DateTime? CollectSonglistDate { get; set; }

    public virtual Songlist Songlist { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
