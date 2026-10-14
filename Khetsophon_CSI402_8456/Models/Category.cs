using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class Category
{
    public int CategoryId { get; set; }

    public string CategoryName { get; set; } = null!;

    public string Slug { get; set; } = null!;

    public bool? IsActive { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
