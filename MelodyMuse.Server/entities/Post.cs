using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.models;

public partial class Post
{
    public decimal PostId { get; set; }

    public string? UserId { get; set; }

    public string? Text { get; set; }

    public string? Title { get; set; }

    public virtual User? User { get; set; }
}
