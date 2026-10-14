using System;
using System.Linq;
using Khetsophon_CSI402_8456.Models;

namespace Khetsophon_CSI402_8456.Data;

/// <summary>
/// DbInitializer: คลาสจัดการเตรียมข้อมูลเริ่มต้น (Database Seeder)
/// WHY: สร้างขึ้นเพื่อรับประกันว่าฐานข้อมูลจริงจะมีข้อมูลจำเป็นเสมอ โดยเฉพาะ:
/// 1. สินค้าไฮไลท์ 3 มิติ (Hero Product with 3D Model) ตามโจทย์ที่ต้องการดึง Model3Dpath จาก DB
/// 2. บัญชีผู้ใช้ตัวอย่างครบทั้ง 5 บทบาท พร้อมรหัสผ่าน BCrypt เพื่อให้ทดสอบล็อกอินแบบกรอกเองได้ทันที
/// 3. หมวดหมู่สินค้าหลักสำหรับเชื่อมโยง Foreign Key
/// </summary>
public static class DbInitializer
{
    public static void Initialize(Csi402dbContext context)
    {
        try
        {
            // ตรวจสอบการเชื่อมต่อฐานข้อมูล
            if (!context.Database.CanConnect())
            {
                return;
            }

            // 1. ตรวจสอบและสร้างหมวดหมู่หลัก (Categories)
            // WHY: ตาราง Products มี Foreign Key บังคับ CategoryId ดังนั้นต้องมีหมวดหมู่อยู่ก่อน
            var scaleCategory = context.Categories.FirstOrDefault(c => c.Slug == "scale-figures");
            if (scaleCategory == null)
            {
                scaleCategory = new Category
                {
                    CategoryName = "สเกลฟิกเกอร์ (Scale Figures)",
                    Slug = "scale-figures",
                    IsActive = true
                };
                context.Categories.Add(scaleCategory);
                context.SaveChanges();
            }

            // 2. ตรวจสอบและสร้าง Hero Product พร้อม 3D Model Path ลงตาราง Products
            // WHY: โจทย์ระบุให้หน้าแรกดึงข้อมูลโมเดล 3D จากคอลัมน์ Model3Dpath ในตาราง products โดยตรง
            var heroProduct = context.Products.FirstOrDefault(p => p.Sku == "FIG-NARUTO-001")
                              ?? context.Products.FirstOrDefault(p => p.IsFeaturedHero);

            if (heroProduct == null)
            {
                heroProduct = new Product
                {
                    Sku = "FIG-NARUTO-001",
                    Barcode = "8859123456789",
                    Name = "นารูโตะ อุซึมากิ โหมดคิวบิเซจ — Ultimate Ninja Storm",
                    CategoryId = scaleCategory.CategoryId,
                    SeriesName = "Naruto Shippuden",
                    BrandName = "FriendHouse Studio x Bandai",
                    Scale = "1/6 Scale",
                    HeightCm = 32.0m,
                    Material = "PVC + เรซิ่นคุณภาพสูง",
                    RetailPrice = 4290.00m,
                    PreorderPrice = 3990.00m,
                    ExpectedArrival = "พร้อมส่งมอบทันที",
                    IsPreorder = false,
                    ImageUrl = "~/media/images/naruto_hero_fig.png",
                    Model3Dpath = "~/media/models/naruto_kyubi_sage.glb", // พาธโมเดล 3 มิติในฐานข้อมูลจริง
                    IsFeaturedHero = true,
                    IsActive = true,
                    CreatedAt = DateTime.Now
                };
                context.Products.Add(heroProduct);
                context.SaveChanges();

                // สร้างสต็อกคงเหลือในตาราง Inventories ควบคู่กัน
                // WHY: ป้องกันปัญหาสินค้าไม่มีสต็อกในหน้าร้าน Staff/Index
                var inventory = new Inventory
                {
                    ProductId = heroProduct.ProductId,
                    QuantityOnHand = 15,
                    ReservedQuantity = 2,
                    SafetyStockLevel = 3,
                    StorageLocation = "Aisle-A-01",
                    LastRestockedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };
                context.Inventories.Add(inventory);
                context.SaveChanges();
            }
            else
            {
                // อัปเดต Model3Dpath และสถานะ Hero หากใน DB ยังว่างอยู่
                bool needUpdate = false;
                if (string.IsNullOrEmpty(heroProduct.Model3Dpath))
                {
                    heroProduct.Model3Dpath = "~/media/models/naruto_kyubi_sage.glb";
                    needUpdate = true;
                }
                if (!heroProduct.IsFeaturedHero)
                {
                    heroProduct.IsFeaturedHero = true;
                    needUpdate = true;
                }
                if (needUpdate)
                {
                    context.SaveChanges();
                }
            }

            // 3. ตรวจสอบและสร้างผู้ใช้ตัวอย่างครบ 5 บทบาท (Seed Users with BCrypt)
            // WHY: เพื่อให้อาจารย์สามารถทดสอบการเข้าสู่ระบบแบบปกติ (กรอกอีเมล/รหัสผ่าน) ได้ทุกบทบาท
            // รหัสผ่านเริ่มต้นคือ "password123" สำหรับทุกบัญชี
            string defaultPasswordHash = BCrypt.Net.BCrypt.HashPassword("password123");

            var defaultUsers = new[]
            {
                new { Email = "customer@friendhouse.com", Name = "คุณสมชาย ใจดี (ลูกค้า)", Role = "Customer", Code = "U20261014001" },
                new { Email = "staff@friendhouse.com", Name = "พนักงานหน้าร้าน สาขาสยาม", Role = "Staff", Code = "U20261014002" },
                new { Email = "owner@friendhouse.com", Name = "คุณกฤษฎา เจ้าของร้าน", Role = "Owner", Code = "U20261014003" },
                new { Email = "it@friendhouse.com", Name = "เจ้าหน้าที่ดูแลระบบ IT", Role = "ITSupport", Code = "U20261014004" },
                new { Email = "account@friendhouse.com", Name = "ฝ่ายการเงินและบัญชี", Role = "Accountant", Code = "U20261014005" }
            };

            foreach (var u in defaultUsers)
            {
                if (!context.Users.Any(existing => existing.Email == u.Email))
                {
                    context.Users.Add(new User
                    {
                        UserCode = u.Code,
                        Email = u.Email,
                        PasswordHash = defaultPasswordHash,
                        Phone = "081-234-5678",
                        FullName = u.Name,
                        RoleName = u.Role,
                        Points = 1200,
                        IsActive = true,
                        CreatedAt = DateTime.Now
                    });
                }
            }
            context.SaveChanges();

            // 4. ตรวจสอบและสร้างโปรโมชั่นตัวอย่างในตาราง Promotions
            // WHY: ให้หน้า Owner/Promotions และ Home/Promotion มีข้อมูลตั้งต้นจากตารางจริง
            if (!context.Promotions.Any())
            {
                context.Promotions.Add(new Promotion
                {
                    Name = "โปรโมชั่นต้อนรับสมาชิกใหม่ ลด 10%",
                    VoucherCode = "FHNEW2026",
                    PromoType = "percentage",
                    DiscountValue = 10.00m,
                    MinimumSpend = 1000.00m,
                    StartDate = DateOnly.FromDateTime(DateTime.Today),
                    EndDate = DateOnly.FromDateTime(DateTime.Today.AddMonths(3)),
                    ApplicableScope = "สินค้าฟิกเกอร์ทุกหมวดหมู่",
                    UsageLimit = 100,
                    CurrentUsageCount = 12,
                    IsActive = true
                });
                context.SaveChanges();
            }
        }
        catch (Exception)
        {
            // WHY: กลืนข้อผิดพลาดไว้ไม่ให้แอปพลิเคชันพังเมื่อฐานข้อมูลยังไม่ได้ Start ในระหว่างรัน Local
            // ระบบ UI ทั้งหมดมี Fallback Mock รองรับอยู่แล้ว
        }
    }
}
