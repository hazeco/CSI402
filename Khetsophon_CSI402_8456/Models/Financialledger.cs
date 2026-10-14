using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Financialledger
{
    public int EntryId { get; set; }

    public string EntryType { get; set; } = null!;

    public string CategoryName { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateOnly EntryDate { get; set; }

    public int RecordedByUserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User RecordedByUser { get; set; } = null!;
}
