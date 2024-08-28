using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.models;

public partial class Album
{
    public string AlbumId { get; set; } = null!;

    public string? AlbumName { get; set; }

    public DateTime? AlbumReleasedate { get; set; }

    public string? AlbumCompany { get; set; }

    public string? AlbumProducer { get; set; }

    public string? ArtistId { get; set; }

    public virtual Artist? Artist { get; set; }

    public virtual ICollection<UserCollectAlbum> UserCollectAlbums { get; set; } = new List<UserCollectAlbum>();

    public virtual ICollection<Song> Songs { get; set; } = new List<Song>();
}
