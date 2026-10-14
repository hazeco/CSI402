using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Receipt
{
    public int ReceiptId { get; set; }

    public string ReceiptNo { get; set; } = null!;

    public int? PreorderId { get; set; }

    public int? CustomerId { get; set; }

    public string? CustomerPhone { get; set; }

    public int CashierUserId { get; set; }

    public decimal SubtotalAmount { get; set; }

    public decimal DiscountAmount { get; set; }

    public decimal VatAmount { get; set; }

    public decimal NetTotalAmount { get; set; }

    public string BarcodeData { get; set; } = null!;

    public DateTime IssuedAt { get; set; }

    public virtual User CashierUser { get; set; } = null!;

    public virtual User? Customer { get; set; }

    public virtual Preorder? Preorder { get; set; }

    public virtual ICollection<Receiptitem> Receiptitems { get; set; } = new List<Receiptitem>();
}
