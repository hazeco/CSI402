using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Rewardredemption
{
    public long RedemptionId { get; set; }

    public string RedemptionCode { get; set; } = null!;

    public int UserId { get; set; }

    public int RewardId { get; set; }

    public int PointsSpent { get; set; }

    public string Status { get; set; } = null!;

    public DateTime RedeemedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public virtual Reward Reward { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
