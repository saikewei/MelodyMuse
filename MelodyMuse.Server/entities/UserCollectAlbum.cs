using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class UserCollectAlbum
{
    public string UserId { get; set; } = null!;

    public string AlbumId { get; set; } = null!;

    public DateTime? CollectAlbumDate { get; set; }

    public virtual Album Album { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
