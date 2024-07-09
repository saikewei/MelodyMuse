using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class Artist
{
    public string ArtistId { get; set; } = null!;

    public string? ArtistName { get; set; }

    public DateTime? ArtistBirthday { get; set; }

    public string? ArtistIntro { get; set; }

    public string? ArtistGenre { get; set; }

    public decimal? ArtistFansNum { get; set; }

    public virtual ICollection<Album> Albums { get; set; } = new List<Album>();

    public virtual ICollection<Song> SongsNavigation { get; set; } = new List<Song>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
