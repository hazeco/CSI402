using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace Khetsophon_CSI402_8456.Models;

public partial class Csi402dbContext : DbContext
{
    public Csi402dbContext()
    {
    }

    public Csi402dbContext(DbContextOptions<Csi402dbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Cartitem> Cartitems { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Financialledger> Financialledgers { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<Labstudent> Labstudents { get; set; }

    public virtual DbSet<Preorder> Preorders { get; set; }

    public virtual DbSet<Preorderitem> Preorderitems { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<Promotion> Promotions { get; set; }

    public virtual DbSet<Receipt> Receipts { get; set; }

    public virtual DbSet<Receiptitem> Receiptitems { get; set; }

    public virtual DbSet<Reward> Rewards { get; set; }

    public virtual DbSet<Rewardredemption> Rewardredemptions { get; set; }

    public virtual DbSet<Systemlog> Systemlogs { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=localhost;port=3306;database=CSI402DB;user=root;password=123456789;charset=utf8mb4", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.46-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Cartitem>(entity =>
        {
            entity.HasKey(e => e.CartItemId).HasName("PRIMARY");

            entity.ToTable("cartitems");

            entity.HasIndex(e => e.ProductId, "fk_cart_product");

            entity.HasIndex(e => new { e.UserId, e.ProductId }, "uq_cart_user_product").IsUnique();

            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Quantity).HasDefaultValueSql("'1'");

            entity.HasOne(d => d.Product).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("fk_cart_product");

            entity.HasOne(d => d.User).WithMany(p => p.Cartitems)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("fk_cart_user");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PRIMARY");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Slug, "uq_categories_slug").IsUnique();

            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Slug).HasMaxLength(100);
        });

        modelBuilder.Entity<Financialledger>(entity =>
        {
            entity.HasKey(e => e.EntryId).HasName("PRIMARY");

            entity.ToTable("financialledger");

            entity.HasIndex(e => e.RecordedByUserId, "fk_ledger_user");

            entity.HasIndex(e => new { e.EntryDate, e.EntryType }, "idx_ledger_date_type");

            entity.Property(e => e.Amount).HasPrecision(12, 2);
            entity.Property(e => e.CategoryName)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.EntryType)
                .HasMaxLength(20)
                .HasDefaultValueSql("'expense'");

            entity.HasOne(d => d.RecordedByUser).WithMany(p => p.Financialledgers)
                .HasForeignKey(d => d.RecordedByUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_ledger_user");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.InventoryId).HasName("PRIMARY");

            entity.ToTable("inventories");

            entity.HasIndex(e => e.ProductId, "uq_inventories_product").IsUnique();

            entity.Property(e => e.AvailableQuantity).HasComputedColumnSql("`QuantityOnHand` - `ReservedQuantity`", true);
            entity.Property(e => e.LastRestockedAt).HasColumnType("datetime");
            entity.Property(e => e.SafetyStockLevel).HasDefaultValueSql("'3'");
            entity.Property(e => e.StorageLocation).HasMaxLength(100);
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Product).WithOne(p => p.Inventory)
                .HasForeignKey<Inventory>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_inv_product");
        });

        modelBuilder.Entity<Labstudent>(entity =>
        {
            entity.HasKey(e => e.StdId).HasName("PRIMARY");

            entity.ToTable("labstudent");

            entity.Property(e => e.StdId)
                .HasMaxLength(10)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.StdLastname)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.StdName)
                .HasMaxLength(50)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.StdPassword)
                .HasMaxLength(30)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Preorder>(entity =>
        {
            entity.HasKey(e => e.PreorderId).HasName("PRIMARY");

            entity.ToTable("preorders");

            entity.HasIndex(e => e.UserId, "fk_preorders_user");

            entity.HasIndex(e => e.ContactPhone, "idx_preorders_phone");

            entity.HasIndex(e => new { e.CurrentStatus, e.EstimatedPickupDate }, "idx_preorders_status");

            entity.HasIndex(e => e.ReservationNumber, "uq_preorders_resno").IsUnique();

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.ContactPhone).HasMaxLength(20);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.CurrentStatus)
                .HasMaxLength(50)
                .HasDefaultValueSql("'PendingConfirm'");
            entity.Property(e => e.PickupBranch)
                .HasMaxLength(100)
                .HasDefaultValueSql("'FriendHouse HQ'");
            entity.Property(e => e.ReservationNumber).HasMaxLength(30);
            entity.Property(e => e.TotalFinalPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.User).WithMany(p => p.Preorders)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_preorders_user");
        });

        modelBuilder.Entity<Preorderitem>(entity =>
        {
            entity.HasKey(e => e.ItemId).HasName("PRIMARY");

            entity.ToTable("preorderitems");

            entity.HasIndex(e => e.PreorderId, "fk_poi_preorder");

            entity.HasIndex(e => e.ProductId, "fk_poi_product");

            entity.Property(e => e.LineTotal)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("`UnitPrice` * `Quantity`", true);
            entity.Property(e => e.Quantity).HasDefaultValueSql("'1'");
            entity.Property(e => e.UnitPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Preorder).WithMany(p => p.Preorderitems)
                .HasForeignKey(d => d.PreorderId)
                .HasConstraintName("fk_poi_preorder");

            entity.HasOne(d => d.Product).WithMany(p => p.Preorderitems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_poi_product");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PRIMARY");

            entity.ToTable("products");

            entity.HasIndex(e => new { e.CategoryId, e.IsActive }, "idx_products_category");

            entity.HasIndex(e => e.IsFeaturedHero, "idx_products_featured");

            entity.HasIndex(e => new { e.IsPreorder, e.IsActive }, "idx_products_preorder");

            entity.HasIndex(e => e.Barcode, "uq_products_barcode").IsUnique();

            entity.HasIndex(e => e.Sku, "uq_products_sku").IsUnique();

            entity.Property(e => e.Barcode).HasMaxLength(50);
            entity.Property(e => e.BrandName)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ExpectedArrival)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.HeightCm).HasPrecision(6, 2);
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.Material).HasMaxLength(100);
            entity.Property(e => e.Model3Dpath)
                .HasMaxLength(255)
                .HasColumnName("Model3DPath");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PreorderDeadline).HasColumnType("datetime");
            entity.Property(e => e.PreorderPrice).HasPrecision(10, 2);
            entity.Property(e => e.RetailPrice).HasPrecision(10, 2);
            entity.Property(e => e.Scale).HasMaxLength(30);
            entity.Property(e => e.SeriesName)
                .HasMaxLength(150)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Sku).HasMaxLength(50);

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_products_category");
        });

        modelBuilder.Entity<Promotion>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PRIMARY");

            entity.ToTable("promotions");

            entity.HasIndex(e => new { e.StartDate, e.EndDate }, "idx_promotions_dates");

            entity.HasIndex(e => e.VoucherCode, "uq_promotions_voucher").IsUnique();

            entity.Property(e => e.ApplicableScope)
                .HasMaxLength(50)
                .HasDefaultValueSql("'all'");
            entity.Property(e => e.DiscountValue).HasPrecision(10, 2);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.MinimumSpend)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'");
            entity.Property(e => e.Name)
                .HasMaxLength(150)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.PromoType).HasMaxLength(50);
            entity.Property(e => e.VoucherCode).HasMaxLength(50);
        });

        modelBuilder.Entity<Receipt>(entity =>
        {
            entity.HasKey(e => e.ReceiptId).HasName("PRIMARY");

            entity.ToTable("receipts");

            entity.HasIndex(e => e.CashierUserId, "fk_rcpt_cashier");

            entity.HasIndex(e => e.PreorderId, "fk_rcpt_preorder");

            entity.HasIndex(e => e.CustomerId, "idx_receipts_cust");

            entity.HasIndex(e => e.IssuedAt, "idx_receipts_issued");

            entity.HasIndex(e => e.ReceiptNo, "uq_receipts_no").IsUnique();

            entity.Property(e => e.BarcodeData).HasMaxLength(100);
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);
            entity.Property(e => e.DiscountAmount).HasPrecision(10, 2);
            entity.Property(e => e.IssuedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.NetTotalAmount).HasPrecision(10, 2);
            entity.Property(e => e.ReceiptNo).HasMaxLength(40);
            entity.Property(e => e.SubtotalAmount).HasPrecision(10, 2);
            entity.Property(e => e.VatAmount).HasPrecision(10, 2);

            entity.HasOne(d => d.CashierUser).WithMany(p => p.ReceiptCashierUsers)
                .HasForeignKey(d => d.CashierUserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rcpt_cashier");

            entity.HasOne(d => d.Customer).WithMany(p => p.ReceiptCustomers)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_rcpt_customer");

            entity.HasOne(d => d.Preorder).WithMany(p => p.Receipts)
                .HasForeignKey(d => d.PreorderId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_rcpt_preorder");
        });

        modelBuilder.Entity<Receiptitem>(entity =>
        {
            entity.HasKey(e => e.ReceiptItemId).HasName("PRIMARY");

            entity.ToTable("receiptitems");

            entity.HasIndex(e => e.ProductId, "fk_ri_product");

            entity.HasIndex(e => e.ReceiptId, "fk_ri_receipt");

            entity.Property(e => e.ItemDescription)
                .HasMaxLength(200)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.LineTotal)
                .HasPrecision(10, 2)
                .HasComputedColumnSql("`UnitPrice` * `Quantity`", true);
            entity.Property(e => e.Quantity).HasDefaultValueSql("'1'");
            entity.Property(e => e.UnitPrice).HasPrecision(10, 2);

            entity.HasOne(d => d.Product).WithMany(p => p.Receiptitems)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_ri_product");

            entity.HasOne(d => d.Receipt).WithMany(p => p.Receiptitems)
                .HasForeignKey(d => d.ReceiptId)
                .HasConstraintName("fk_ri_receipt");
        });

        modelBuilder.Entity<Reward>(entity =>
        {
            entity.HasKey(e => e.RewardId).HasName("PRIMARY");

            entity.ToTable("rewards");

            entity.HasIndex(e => e.RewardCode, "uq_rewards_code").IsUnique();

            entity.Property(e => e.Brand)
                .HasMaxLength(100)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.CategoryTag)
                .HasMaxLength(50)
                .HasDefaultValueSql("'Merchandise'");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.ImageUrl).HasMaxLength(255);
            entity.Property(e => e.IsAvailable)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.RewardCode).HasMaxLength(30);
            entity.Property(e => e.Title)
                .HasMaxLength(150)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        modelBuilder.Entity<Rewardredemption>(entity =>
        {
            entity.HasKey(e => e.RedemptionId).HasName("PRIMARY");

            entity.ToTable("rewardredemptions");

            entity.HasIndex(e => e.RewardId, "fk_rdm_reward");

            entity.HasIndex(e => e.UserId, "idx_redemptions_user");

            entity.HasIndex(e => e.RedemptionCode, "uq_redemptions_code").IsUnique();

            entity.Property(e => e.CompletedAt).HasColumnType("datetime");
            entity.Property(e => e.RedeemedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.RedemptionCode).HasMaxLength(40);
            entity.Property(e => e.Status)
                .HasMaxLength(30)
                .HasDefaultValueSql("'PendingPickup'");

            entity.HasOne(d => d.Reward).WithMany(p => p.Rewardredemptions)
                .HasForeignKey(d => d.RewardId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rdm_reward");

            entity.HasOne(d => d.User).WithMany(p => p.Rewardredemptions)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("fk_rdm_user");
        });

        modelBuilder.Entity<Systemlog>(entity =>
        {
            entity.HasKey(e => e.LogId).HasName("PRIMARY");

            entity.ToTable("systemlogs");

            entity.HasIndex(e => e.UserId, "fk_logs_user");

            entity.HasIndex(e => new { e.LogLevel, e.Timestamp }, "idx_logs_level").IsDescending(false, true);

            entity.HasIndex(e => e.Module, "idx_logs_module");

            entity.HasIndex(e => e.LogCode, "uq_logs_code").IsUnique();

            entity.Property(e => e.IpAddress).HasMaxLength(50);
            entity.Property(e => e.LogCode).HasMaxLength(30);
            entity.Property(e => e.LogLevel).HasMaxLength(20);
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.Module).HasMaxLength(100);
            entity.Property(e => e.Timestamp)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");

            entity.HasOne(d => d.User).WithMany(p => p.Systemlogs)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_logs_user");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.ToTable("users");

            entity.HasIndex(e => e.Phone, "idx_users_phone");

            entity.HasIndex(e => new { e.RoleName, e.IsActive }, "idx_users_role_active");

            entity.HasIndex(e => e.Email, "uq_users_email").IsUnique();

            entity.HasIndex(e => e.UserCode, "uq_users_usercode").IsUnique();

            entity.Property(e => e.AvatarUrl).HasMaxLength(255);
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime");
            entity.Property(e => e.Email).HasMaxLength(150);
            entity.Property(e => e.FullName)
                .HasMaxLength(150)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("'1'");
            entity.Property(e => e.PasswordHash).HasMaxLength(255);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.RoleName)
                .HasMaxLength(30)
                .HasDefaultValueSql("'Customer'");
            entity.Property(e => e.UserCode)
                .HasMaxLength(30)
                .UseCollation("utf8mb3_general_ci")
                .HasCharSet("utf8mb3");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
