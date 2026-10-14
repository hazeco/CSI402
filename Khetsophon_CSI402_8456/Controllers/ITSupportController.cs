using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;

namespace Khetsophon_CSI402_8456.Controllers;

public class ITSupportController : Controller
{
    private readonly Csi402dbContext _db;

    public ITSupportController(Csi402dbContext db)
    {
        _db = db;
    }

    private bool IsAuthorizedIT()
    {
        var role = HttpContext.Session.GetString("Role");
        return role != null && (role.ToLower() == "itsupport" || role.ToLower() == "owner");
    }

    public IActionResult Index()
    {
        return RedirectToAction("Dashboard");
    }

    // READ: ภาพรวมสถานะระบบ (Dashboard)
    public IActionResult Dashboard()
    {
        if (!IsAuthorizedIT())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชี IT Support";
            return RedirectToAction("Login", "Account");
        }

        ViewBag.TotalUsers = _db.Users.Count();
        ViewBag.TotalLogs = _db.Systemlogs.Count();
        ViewBag.ErrorLogs = _db.Systemlogs.Count(l => l.LogLevel == "ERROR");
        ViewBag.TotalProducts = _db.Products.Count();

        return View();
    }

    // READ: จัดการบัญชีผู้ใช้งาน (Users)
    public IActionResult Users(string? searchQuery, string? statusFilter)
    {
        if (!IsAuthorizedIT())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชี IT Support";
            return RedirectToAction("Login", "Account");
        }

        var query = _db.Users.AsQueryable();

        if (!string.IsNullOrEmpty(searchQuery))
        {
            query = query.Where(u => u.Email.Contains(searchQuery) || u.Phone.Contains(searchQuery) || (u.FullName != null && u.FullName.Contains(searchQuery)));
        }

        if (!string.IsNullOrEmpty(statusFilter) && statusFilter != "all")
        {
            if (statusFilter == "ปกติ")
            {
                query = query.Where(u => u.IsActive == true);
            }
            else if (statusFilter == "ถูกล็อก")
            {
                query = query.Where(u => u.IsActive == false);
            }
        }

        var users = query.OrderByDescending(u => u.CreatedAt).ToList();
        ViewBag.SearchQuery = searchQuery;
        ViewBag.StatusFilter = statusFilter;

        return View(users);
    }

    // CREATE: เพิ่มผู้ใช้งานใหม่ (Create User)
    [HttpPost]
    public IActionResult CreateUser(string role, string? fullName, string email, string phone, string password)
    {
        if (!IsAuthorizedIT())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชี IT Support";
            return RedirectToAction("Login", "Account");
        }

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(phone))
        {
            TempData["ErrorMessage"] = "กรุณากรอกข้อมูลให้ครบถ้วน";
            return RedirectToAction("Users");
        }

        var checkUser = _db.Users.FirstOrDefault(u => u.Email == email);
        if (checkUser != null)
        {
            TempData["ErrorMessage"] = "อีเมลนี้มีอยู่ในระบบแล้ว";
            return RedirectToAction("Users");
        }

        var newUser = new User
        {
            UserCode = "U" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            RoleName = string.IsNullOrEmpty(role) ? "customer" : role.ToLower(),
            FullName = fullName,
            Email = email,
            Phone = phone,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
            Points = 0,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _db.Users.Add(newUser);

        // บันทึก Log เข้าระบบ
        _db.Systemlogs.Add(new Systemlog
        {
            LogCode = "LOG-" + DateTime.Now.ToString("yyyyMMddHHmmss"),
            LogLevel = "INFO",
            Module = "auth.users",
            Message = "IT Support สร้างผู้ใช้งานใหม่: " + email + " (สิทธิ์: " + newUser.RoleName + ")",
            Timestamp = DateTime.UtcNow
        });

        _db.SaveChanges();

        TempData["SuccessMessage"] = "สร้างบัญชีผู้ใช้งาน \"" + email + "\" เรียบร้อยแล้ว";
        return RedirectToAction("Users");
    }



    // READ: บันทึกกิจกรรมและข้อผิดพลาด (System Logs)
    public IActionResult Logs(string? level, string? timeRange)
    {
        if (!IsAuthorizedIT())
        {
            TempData["ErrorMessage"] = "กรุณาเข้าสู่ระบบด้วยบัญชี IT Support";
            return RedirectToAction("Login", "Account");
        }

        var query = _db.Systemlogs.AsQueryable();

        if (!string.IsNullOrEmpty(level) && level != "all")
        {
            query = query.Where(l => l.LogLevel == level);
        }

        var logs = query.OrderByDescending(l => l.Timestamp).Take(100).ToList();
        ViewBag.SelectedLevel = level ?? "all";
        ViewBag.SelectedTimeRange = timeRange ?? "24h";

        return View(logs);
    }
}
