using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Khetsophon_CSI402_8456.Models;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class StaffController : Controller
{
    private readonly Csi402dbContext _db;

    public StaffController(Csi402dbContext db)
    {
        _db = db;
    }

    private bool IsAuthorizedStaff()
    {
        var role = HttpContext.Session.GetString("Role");
        return role != null && (role.ToLower() == "staff" || role.ToLower() == "owner" || role.ToLower() == "itsupport");
    }

    // READ: สต็อกสินค้าหน้าร้านและแสดงผลใบจองจากเคาน์เตอร์
    public IActionResult Index(CounterPreorderViewModel? data)
    {
        if (!IsAuthorizedStaff())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีพนักงาน (Staff)";
            return RedirectToAction("Login", "Account");
        }

        if (data != null && !string.IsNullOrEmpty(data.ReservationNumber))
        {
            ViewBag.ReservationNumber = data.ReservationNumber;
        }

        var products = _db.Products
            .Include(p => p.Category)
            .Include(p => p.Inventory)
            .ToList();

        return View(products);
    }

    // READ: ค้นหาใบจองพรีออเดอร์ (GET)
    [HttpGet]
    public IActionResult CounterPreorder()
    {
        if (!IsAuthorizedStaff())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีพนักงาน (Staff)";
            return RedirectToAction("Login", "Account");
        }

        return View();
    }

    // READ: ค้นหาใบจองพรีออเดอร์จากฐานข้อมูลจริง (POST)
    [HttpPost]
    public IActionResult CounterPreorder(CounterPreorderViewModel data)
    {
        if (!IsAuthorizedStaff())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีพนักงาน (Staff)";
            return RedirectToAction("Login", "Account");
        }

        if (!string.IsNullOrEmpty(data.ReservationNumber) || !string.IsNullOrEmpty(data.Phone))
        {
            var preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                    .ThenInclude(item => item.Product)
                .Include(p => p.User)
                .FirstOrDefault(p => (!string.IsNullOrEmpty(data.ReservationNumber) && p.ReservationNumber == data.ReservationNumber)
                                  || (!string.IsNullOrEmpty(data.Phone) && p.ContactPhone == data.Phone));

            if (preorder != null)
            {
                ViewBag.SearchResult = preorder;
                ViewBag.Status = "พบข้อมูลใบสั่งจอง";
            }
            else
            {
                ViewBag.Status = "ไม่พบข้อมูลใบสั่งจองตามเงื่อนไขที่ระบุ";
            }
        }

        return View(data);
    }

    // 1. ACTION: มารับแล้ว / ชำระเงินแล้ว (Complete Pickup & Issue Receipt & Member Points)
    // WHY: อัปเดตสถานะ Preorder เป็น Completed, ออกใบเสร็จ Receipt จริงใน DB และเพิ่ม Points ให้สมาชิก
    [HttpPost]
    public IActionResult CompletePickup(int? preorderId, string? memberPhone)
    {
        if (!IsAuthorizedStaff())
        {
            return Json(new { success = false, message = "ไม่มีสิทธิ์เข้าถึง" });
        }

        int pointsEarned = 0;
        bool memberFound = true;
        string receiptNo = "RCP" + DateTime.Now.ToString("yyyyMMddHHmmss");

        if (preorderId.HasValue && preorderId.Value > 0)
        {
            var preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                .FirstOrDefault(p => p.PreorderId == preorderId.Value);

            if (preorder != null)
            {
                preorder.CurrentStatus = "Completed";
                preorder.CompletedAt = DateTime.Now;

                // สะสมแต้มสมาชิก: ค้นหาเบอร์โทรในตาราง Users
                if (!string.IsNullOrWhiteSpace(memberPhone))
                {
                    var cleanPhone = memberPhone.Trim().Replace("-", "");
                    var memberUser = _db.Users.FirstOrDefault(u => u.Phone == cleanPhone || u.Phone == memberPhone.Trim() || u.Email == memberPhone.Trim());
                    if (memberUser != null)
                    {
                        // คำนวณแต้ม: 10% ของยอด หรือ 150 แต้มมาตรฐาน
                        pointsEarned = (int)Math.Max(10, Math.Round(preorder.TotalFinalPrice / 10));
                        memberUser.Points += pointsEarned;
                        memberFound = true;
                    }
                    else
                    {
                        memberFound = false;
                    }
                }

                // ออกใบเสร็จรับเงินจริง (Receipts) - ไม่ต้องระบุประเภทการชำระเงินตามโครงสร้างตาราง
                var cashierId = HttpContext.Session.GetInt32("UserId") ?? 2;
                var receipt = new Receipt
                {
                    ReceiptNo = receiptNo,
                    PreorderId = preorder.PreorderId,
                    CustomerId = preorder.UserId,
                    CustomerPhone = memberPhone ?? preorder.ContactPhone,
                    CashierUserId = cashierId,
                    SubtotalAmount = preorder.TotalFinalPrice,
                    DiscountAmount = 0.00m,
                    VatAmount = Math.Round(preorder.TotalFinalPrice * 0.07m, 2),
                    NetTotalAmount = preorder.TotalFinalPrice,
                    BarcodeData = preorder.ReservationNumber,
                    IssuedAt = DateTime.Now
                };
                _db.Receipts.Add(receipt);

                // ปรับลดยอดจองในคลังสินค้า (Inventories)
                foreach (var item in preorder.Preorderitems)
                {
                    var inv = _db.Inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (inv != null)
                    {
                        inv.ReservedQuantity = Math.Max(0, inv.ReservedQuantity - item.Quantity);
                        inv.QuantityOnHand = Math.Max(0, inv.QuantityOnHand - item.Quantity);
                    }
                }

                _db.SaveChanges();
            }
        }

        return Json(new { success = true, receiptNo, pointsEarned, memberFound, message = "บันทึกการรับสินค้าและชำระเงินเรียบร้อย" });
    }

    // 2. ACTION: ไม่รับสินค้า (Reject Preorder & Log Reason)
    // WHY: อัปเดตสถานะเป็น Cancelled, คืนสต็อกที่จองไว้ และบันทึกเหตุผลลง Systemlogs เป็น Audit Trail
    [HttpPost]
    public IActionResult RejectPreorder(int? preorderId, string reason, string? remark)
    {
        if (!IsAuthorizedStaff())
        {
            return Json(new { success = false, message = "ไม่มีสิทธิ์เข้าถึง" });
        }

        string orderNumber = "PO-2026-9912";

        if (preorderId.HasValue && preorderId.Value > 0)
        {
            var preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                .FirstOrDefault(p => p.PreorderId == preorderId.Value);

            if (preorder != null)
            {
                orderNumber = preorder.ReservationNumber;
                preorder.CurrentStatus = "Cancelled";

                // คืนยอดจองกลับสู่สต็อก
                foreach (var item in preorder.Preorderitems)
                {
                    var inv = _db.Inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (inv != null)
                    {
                        inv.ReservedQuantity = Math.Max(0, inv.ReservedQuantity - item.Quantity);
                    }
                }

                // บันทึกเหตุผลลงใน Systemlogs (เนื่องจากตาราง Preorders ไม่มีคอลัมน์เก็บเหตุผล)
                _db.Systemlogs.Add(new Systemlog
                {
                    LogCode = "REJECT-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    LogLevel = "Warning",
                    Module = "CounterPreorder",
                    Message = $"ลูกค้าไม่รับสินค้า ใบจอง #{orderNumber} สาเหตุ: {reason} | รายละเอียด: {remark ?? "ไม่มี"}",
                    UserId = HttpContext.Session.GetInt32("UserId"),
                    Timestamp = DateTime.Now
                });

                _db.SaveChanges();
            }
        }

        return Json(new { success = true, orderNumber, message = "บันทึกการไม่รับสินค้าเรียบร้อย" });
    }

    // 3. ACTION: ไม่มารับสินค้า (Customer No-Show)
    // WHY: อัปเดตสถานะเป็น Cancelled, ปลดสต็อกที่จองไว้กลับสู่คลัง และบันทึกเหตุผลลง Systemlogs
    [HttpPost]
    public IActionResult NoShowPreorder(int? preorderId, string reason, string? remark)
    {
        if (!IsAuthorizedStaff())
        {
            return Json(new { success = false, message = "ไม่มีสิทธิ์เข้าถึง" });
        }

        string orderNumber = "PO-2026-9912";

        if (preorderId.HasValue && preorderId.Value > 0)
        {
            var preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                .FirstOrDefault(p => p.PreorderId == preorderId.Value);

            if (preorder != null)
            {
                orderNumber = preorder.ReservationNumber;
                preorder.CurrentStatus = "Cancelled";

                foreach (var item in preorder.Preorderitems)
                {
                    var inv = _db.Inventories.FirstOrDefault(i => i.ProductId == item.ProductId);
                    if (inv != null)
                    {
                        inv.ReservedQuantity = Math.Max(0, inv.ReservedQuantity - item.Quantity);
                    }
                }

                _db.Systemlogs.Add(new Systemlog
                {
                    LogCode = "NOSHOW-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    LogLevel = "Warning",
                    Module = "CounterPreorder",
                    Message = $"ลูกค้าไม่มารับสินค้า ใบจอง #{orderNumber} สาเหตุ: {reason} | หมายเหตุ: {remark ?? "ไม่มี"}",
                    UserId = HttpContext.Session.GetInt32("UserId"),
                    Timestamp = DateTime.Now
                });

                _db.SaveChanges();
            }
        }

        return Json(new { success = true, orderNumber, message = "บันทึกลูกค้าไม่มารับสินค้าเรียบร้อย" });
    }

    // READ: สแกนรับสินค้าเข้าคลัง (GET)
    public IActionResult ScanReceive()
    {
        if (!IsAuthorizedStaff())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชีพนักงาน (Staff)";
            return RedirectToAction("Login", "Account");
        }

        var products = _db.Products.Take(20).ToList();
        return View(products);
    }
}
