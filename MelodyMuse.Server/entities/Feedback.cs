using System;
using System.Collections.Generic;

namespace MelodyMuse.Server.Models;

public partial class Feedback
{
    public string? UserId { get; set; }

    public decimal FeedbackId { get; set; }

    public string? Substance { get; set; }

    public DateTime? Time { get; set; }

    public decimal? ProcessingStatus { get; set; }

    public string? Reply { get; set; }

    public virtual User? User { get; set; }
}
