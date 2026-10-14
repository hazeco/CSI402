using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Receiptitem
{
    public int ReceiptItemId { get; set; }

    public int ReceiptId { get; set; }

    public int? ProductId { get; set; }

    public string ItemDescription { get; set; } = null!;

    public int Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal? LineTotal { get; set; }

    public virtual Product? Product { get; set; }

    public virtual Receipt Receipt { get; set; } = null!;
}
