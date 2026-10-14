using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Systemlog
{
    public long LogId { get; set; }

    public string LogCode { get; set; } = null!;

    public string LogLevel { get; set; } = null!;

    public string Module { get; set; } = null!;

    public string Message { get; set; } = null!;

    public string? DetailPayload { get; set; }

    public string? IpAddress { get; set; }

    public int? UserId { get; set; }

    public DateTime Timestamp { get; set; }

    public virtual User? User { get; set; }
}
