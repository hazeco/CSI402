using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Preorderitem
{
    public int ItemId { get; set; }

    public int PreorderId { get; set; }

    public int ProductId { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual Preorder Preorder { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
