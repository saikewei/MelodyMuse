using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class Upload
{
    public string SongId { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public DateTime? UploadDate { get; set; }

    public virtual Song Song { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
