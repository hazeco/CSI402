using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Khetsophon_CSI402_8456.Models;

namespace Khetsophon_CSI402_8456.Controllers;

public class AccountantController : Controller
{
    private readonly Csi402dbContext _db;

    public AccountantController(Csi402dbContext db)
    {
        _db = db;
    }

    private bool IsAuthorizedAccountant()
    {
        var role = HttpContext.Session.GetString("Role");
        return role != null && (role.ToLower() == "accountant" || role.ToLower() == "owner" || role.ToLower() == "itsupport");
    }

    // READ: แดชบอร์ดสรุปการเงิน
    public IActionResult Index()
    {
        if (!IsAuthorizedAccountant())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีผู้ทำบัญชี (Accountant)";
            return RedirectToAction("Login", "Account");
        }

        var receipts = _db.Receipts.ToList();
        var ledgers = _db.Financialledgers.ToList();

        var storefrontRevenue = receipts.Sum(r => r.NetTotalAmount);
        var ledgerIncome = ledgers.Where(l => l.EntryType.ToLower() == "income").Sum(l => l.Amount);
        var totalExpenses = ledgers.Where(l => l.EntryType.ToLower() == "expense").Sum(l => l.Amount);
        var preordersPending = _db.Preorders.Where(p => p.CurrentStatus == "Pending" || p.CurrentStatus == "รอชำระ").Sum(p => p.TotalFinalPrice);

        ViewBag.TotalMonthlyRevenue = storefrontRevenue + ledgerIncome;
        ViewBag.StorefrontRevenue = storefrontRevenue;
        ViewBag.RecordedExpenses = totalExpenses;
        ViewBag.PreordersPendingTotal = preordersPending;
        ViewBag.ReceiptCount = receipts.Count;

        return View();
    }

    // READ: ประวัติใบเสร็จและธุรกรรมทั้งหมด
    public IActionResult Receipts(string? searchTerm)
    {
        if (!IsAuthorizedAccountant())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีผู้ทำบัญชี (Accountant)";
            return RedirectToAction("Login", "Account");
        }

        var query = _db.Receipts
            .Include(r => r.Receiptitems)
            .Include(r => r.CashierUser)
            .Include(r => r.Customer)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(r => r.ReceiptNo.Contains(searchTerm) || (r.CustomerPhone != null && r.CustomerPhone.Contains(searchTerm)));
        }

        var receipts = query.OrderByDescending(r => r.IssuedAt).ToList();
        ViewBag.SearchTerm = searchTerm;

        return View(receipts);
    }

    // READ: สมุดบันทึกรายรับ-รายจ่าย (Ledger)
    public IActionResult Ledger()
    {
        if (!IsAuthorizedAccountant())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีผู้ทำบัญชี (Accountant)";
            return RedirectToAction("Login", "Account");
        }

        var entries = _db.Financialledgers
            .Include(f => f.RecordedByUser)
            .OrderByDescending(f => f.EntryDate)
            .ToList();

        var totalIncome = entries.Where(e => e.EntryType.ToLower() == "income").Sum(e => e.Amount);
        var totalExpense = entries.Where(e => e.EntryType.ToLower() == "expense").Sum(e => e.Amount);

        ViewBag.TotalIncome = totalIncome;
        ViewBag.TotalExpense = totalExpense;
        ViewBag.NetBalance = totalIncome - totalExpense;

        return View(entries);
    }

    // CREATE: บันทึกรายการรายรับ-รายจ่ายใหม่ลงตาราง Financialledgers
    public IActionResult AddLedgerEntry(string? EntryType, string? CategoryName, string? Note, decimal? Amount, DateOnly? EntryDate)
    {
        if (!IsAuthorizedAccountant())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีผู้ทำบัญชี (Accountant)";
            return RedirectToAction("Login", "Account");
        }

        if (Amount.HasValue && Amount.Value > 0 && !string.IsNullOrEmpty(CategoryName))
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (!userId.HasValue)
            {
                // หากไม่มี session ให้ใช้ User คนแรกในระบบ
                var defaultUser = _db.Users.FirstOrDefault();
                userId = defaultUser != null ? defaultUser.UserId : 1;
            }

            var type = string.IsNullOrEmpty(EntryType) ? "expense" : EntryType.ToLower();

            var newEntry = new Financialledger
            {
                EntryType = type,
                CategoryName = CategoryName,
                Description = Note ?? "",
                Amount = Amount.Value,
                EntryDate = EntryDate ?? DateOnly.FromDateTime(DateTime.Today),
                RecordedByUserId = userId.Value,
                CreatedAt = DateTime.UtcNow
            };

            _db.Financialledgers.Add(newEntry);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "บันทึกรายการ \"" + CategoryName + "\" จำนวน ฿" + Amount.Value.ToString("N2") + " เรียบร้อยแล้ว";
            return RedirectToAction("Ledger");
        }

        return RedirectToAction("Ledger");
    }
}
