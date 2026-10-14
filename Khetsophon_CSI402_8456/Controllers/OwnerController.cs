using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;

namespace Khetsophon_CSI402_8456.Controllers;

public class OwnerController : Controller
{
    private readonly Csi402dbContext _db;

    public OwnerController(Csi402dbContext db)
    {
        _db = db;
    }

    private bool IsAuthorizedOwner()
    {
        var role = HttpContext.Session.GetString("Role");
        return role != null && (role.ToLower() == "owner" || role.ToLower() == "itsupport");
    }

    public IActionResult Index()
    {
        return RedirectToAction("Promotions");
    }

    // READ: รายการโปรโมชั่นทั้งหมดจากตาราง Promotions
    public IActionResult Promotions()
    {
        if (!IsAuthorizedOwner())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีเจ้าของร้าน (Owner)";
            return RedirectToAction("Login", "Account");
        }

        var promotions = _db.Promotions.ToList();
        return View(promotions);
    }

    // CREATE: หน้าแบบฟอร์มและการบันทึกโปรโมชั่นใหม่
    public IActionResult AddPromotion(Promotion? data)
    {
        if (!IsAuthorizedOwner())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีเจ้าของร้าน (Owner)";
            return RedirectToAction("Login", "Account");
        }

        if (data != null && !string.IsNullOrEmpty(data.Name))
        {
            if (string.IsNullOrEmpty(data.VoucherCode))
            {
                data.VoucherCode = "FH-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            }

            if (string.IsNullOrEmpty(data.PromoType))
            {
                data.PromoType = "percent";
            }

            if (string.IsNullOrEmpty(data.ApplicableScope))
            {
                data.ApplicableScope = "all";
            }

            data.CurrentUsageCount = 0;
            data.IsActive = data.IsActive ?? true;

            _db.Promotions.Add(data);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "เพิ่มโปรโมชั่นใหม่ \"" + data.Name + "\" เรียบร้อยแล้ว";
            return RedirectToAction("Promotions");
        }

        return View();
    }

    public IActionResult EditPromotion()
    {
        if (!IsAuthorizedOwner())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีเจ้าของร้าน (Owner)";
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    // READ: หน้ารายงานสรุปยอดประจำเดือน
    public IActionResult MonthlyReport()
    {
        if (!IsAuthorizedOwner())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีเจ้าของร้าน (Owner)";
            return RedirectToAction("Login", "Account");
        }

        // ดึงยอดรวมจากใบเสร็จ
        var receipts = _db.Receipts.ToList();
        ViewBag.TotalSales = receipts.Sum(r => r.NetTotalAmount);
        ViewBag.TotalReceiptCount = receipts.Count;

        return View();
    }
}
