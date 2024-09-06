using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class ArtistFanCount
{
    public string? ArtistId { get; set; }

    public decimal? FanCount { get; set; }
}
