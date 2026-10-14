using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Reward
{
    public int RewardId { get; set; }

    public string RewardCode { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string? Brand { get; set; }

    public string CategoryTag { get; set; } = null!;

    public int PointsRequired { get; set; }

    public int StockQuantity { get; set; }

    public string ImageUrl { get; set; } = null!;

    public bool? IsAvailable { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Rewardredemption> Rewardredemptions { get; set; } = new List<Rewardredemption>();
}
