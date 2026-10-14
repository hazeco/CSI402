using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Labstudent
{
    public string StdId { get; set; } = null!;

    public string? StdPassword { get; set; }

    public string? StdName { get; set; }

    public string? StdLastname { get; set; }
}
