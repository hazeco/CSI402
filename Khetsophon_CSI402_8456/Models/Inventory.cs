using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Inventory
{
    public int InventoryId { get; set; }

    public int ProductId { get; set; }

    public int QuantityOnHand { get; set; }

    public int ReservedQuantity { get; set; }

    public int? AvailableQuantity { get; set; }

    public int SafetyStockLevel { get; set; }

    public string? StorageLocation { get; set; }

    public DateTime? LastRestockedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public virtual Product Product { get; set; } = null!;
}
