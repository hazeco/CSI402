using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string Sku { get; set; } = null!;

    public string? Barcode { get; set; }

    public string Name { get; set; } = null!;

    public int CategoryId { get; set; }

    public string? SeriesName { get; set; }

    public string? BrandName { get; set; }

    public string? Scale { get; set; }

    public decimal? HeightCm { get; set; }

    public string? Material { get; set; }

    public decimal RetailPrice { get; set; }

    public decimal? PreorderPrice { get; set; }

    public string? ExpectedArrival { get; set; }

    public bool IsPreorder { get; set; }

    public DateTime? PreorderDeadline { get; set; }

    public string ImageUrl { get; set; } = null!;

    public string? Model3Dpath { get; set; }

    public bool IsFeaturedHero { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual Category Category { get; set; } = null!;

    public virtual Inventory? Inventory { get; set; }

    public virtual ICollection<Preorderitem> Preorderitems { get; set; } = new List<Preorderitem>();

    public virtual ICollection<Receiptitem> Receiptitems { get; set; } = new List<Receiptitem>();
}
