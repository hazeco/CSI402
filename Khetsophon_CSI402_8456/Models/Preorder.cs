using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Preorder
{
    public int PreorderId { get; set; }

    public string ReservationNumber { get; set; } = null!;

    public int UserId { get; set; }

    public string ContactPhone { get; set; } = null!;

    public DateOnly EstimatedPickupDate { get; set; }

    public string CurrentStatus { get; set; } = null!;

    public decimal TotalFinalPrice { get; set; }

    public string PickupBranch { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual ICollection<Preorderitem> Preorderitems { get; set; } = new List<Preorderitem>();

    public virtual ICollection<Receipt> Receipts { get; set; } = new List<Receipt>();

    public virtual User User { get; set; } = null!;
}
