using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Cartitem
{
    public int CartItemId { get; set; }

    public int UserId { get; set; }

    public int ProductId { get; set; }

    public int Quantity { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
