using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Promotion
{
    public int PromotionId { get; set; }

    public string Name { get; set; } = null!;

    public string VoucherCode { get; set; } = null!;

    public string PromoType { get; set; } = null!;

    public decimal? DiscountValue { get; set; }

    public decimal? MinimumSpend { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string ApplicableScope { get; set; } = null!;

    public int? UsageLimit { get; set; }

    public int CurrentUsageCount { get; set; }

    public bool? IsActive { get; set; }
}
