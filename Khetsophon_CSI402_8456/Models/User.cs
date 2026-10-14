using System;
using System.Collections.Generic;

namespace Khetsophon_CSI402_8456.Models;

public partial class User
{
    public int UserId { get; set; }

    public string UserCode { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string? FullName { get; set; }

    public string? AvatarUrl { get; set; }

    public string RoleName { get; set; } = null!;

    public int Points { get; set; }

    public bool? IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Cartitem> Cartitems { get; set; } = new List<Cartitem>();

    public virtual ICollection<Financialledger> Financialledgers { get; set; } = new List<Financialledger>();

    public virtual ICollection<Preorder> Preorders { get; set; } = new List<Preorder>();

    public virtual ICollection<Receipt> ReceiptCashierUsers { get; set; } = new List<Receipt>();

    public virtual ICollection<Receipt> ReceiptCustomers { get; set; } = new List<Receipt>();

    public virtual ICollection<Rewardredemption> Rewardredemptions { get; set; } = new List<Rewardredemption>();

    public virtual ICollection<Systemlog> Systemlogs { get; set; } = new List<Systemlog>();
}
