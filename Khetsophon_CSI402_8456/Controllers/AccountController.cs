using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using Khetsophon_CSI402_8456.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Khetsophon_CSI402_8456.Controllers;

public class AccountController : Controller
{
    private readonly Csi402dbContext _db;

    public AccountController(Csi402dbContext db)
    {
        _db = db;
    }
    
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Register(RegisterViewModel data)
    {
        if (ModelState.IsValid)
        {
            // 1. READ: check email exits
            var checkUser = _db.Users.FirstOrDefault(u => u.Email == data.Email);
            
            if (checkUser == null)
            {
                // 2.CREATE: new user
                User newUser =  new User();
                newUser.UserCode = "U" + DateTime.Now.ToString("yyyyMMddHHmmss");
                newUser.Email = data.Email;
                newUser.Phone = data.Phone;
                newUser.PasswordHash = BCrypt.Net.BCrypt.HashPassword(data.Password);
                newUser.FullName = null;
                newUser.AvatarUrl = null;
                newUser.RoleName = "customer";
                newUser.Points = 0;
                newUser.IsActive = true;
                newUser.CreatedAt = DateTime.UtcNow;
                _db.Users.Add(newUser);
                _db.SaveChanges();

                // send data to login page for login process
                TempData["SuccessMessage"] = "Registered Successfully. Now you can login.";
                return RedirectToAction("Login", "Account");
            } else
            {
                ViewBag.Status = "Email is already exists";
                return View(data);
            }
        }
        return View(data);
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Login(LoginViewModel data)
    {
        if (ModelState.IsValid)
        {
            // READ: ค้นหาผู้ใช้จาก Email ในฐานข้อมูล
            var user = _db.Users.FirstOrDefault(u => u.Email == data.Email);
            
            // ตรวจสอบรหัสผ่านด้วย BCrypt
            if (user != null && BCrypt.Net.BCrypt.Verify(data.Password, user.PasswordHash))
            {
                // save status in session
                HttpContext.Session.SetInt32("UserId", user.UserId);
                HttpContext.Session.SetString("Role", user.RoleName);
                HttpContext.Session.SetString("UserEmail", user.Email);
                HttpContext.Session.SetString("UserName", user.FullName ?? user.Email);
                
                // Login สำเร็จ
                return RedirectToAction("Index", "Home" );
            }
            else
            {
                ViewBag.Status = "Email or password is incorrect";
                return View(data);
            }
        }
        return View(data);
    }

    // FAST LOGIN (MOCK LOGIN): ระบบล็อกอินด่วนสำหรับการทดสอบและนำเสนองาน
    // WHY: ออกแบบมาเพื่อให้อาจารย์และผู้ตรวจสามารถสลับบทบาท (Role) ได้ใน 1 คลิก
    // WHY SESSION: แม้จะเป็น Mock Login แต่จำเป็นต้องบันทึก Session จริง (UserId, Role, UserEmail, UserName)
    // เพื่อให้ตัวตรวจสิทธิ์ (Route Guard) ใน Controller หลังบ้าน (Staff, Owner, IT, Accountant) ปล่อยให้ผ่านได้จริง
    [HttpGet]
    public IActionResult FastLogin(string role)
    {
        string normalizedRole = (role ?? "customer").ToLowerInvariant();
        
        int mockUserId = 1;
        string email = "customer@friendhouse.com";
        string name = "ลูกค้าทดสอบ (Demo Customer)";
        string targetAction = "Customer";
        string targetController = "Home";

        switch (normalizedRole)
        {
            case "staff":
                mockUserId = 2;
                email = "staff@friendhouse.com";
                name = "พนักงานหน้าร้าน (Demo Staff)";
                targetController = "Staff";
                targetAction = "Index";
                break;

            case "owner":
                mockUserId = 3;
                email = "owner@friendhouse.com";
                name = "เจ้าของร้าน (Demo Owner)";
                targetController = "Owner";
                targetAction = "Promotions";
                break;

            case "itsupport":
                mockUserId = 4;
                email = "it@friendhouse.com";
                name = "เจ้าหน้าที่ไอที (Demo IT Support)";
                targetController = "ITSupport";
                targetAction = "Dashboard";
                break;

            case "accountant":
                mockUserId = 5;
                email = "account@friendhouse.com";
                name = "นักบัญชี (Demo Accountant)";
                targetController = "Accountant";
                targetAction = "Index";
                break;

            default:
                normalizedRole = "customer";
                break;
        }

        // WHY: ค้นหาใน DB ก่อนเพื่อใช้ ID จริงหากมีการ Seed ไว้ แต่ถ้ายังไม่มีก็มี Fallback ค่าจำลองให้ระบบไม่พัง
        var dbUser = _db.Users.FirstOrDefault(u => u.RoleName.ToLower() == normalizedRole);
        if (dbUser != null)
        {
            mockUserId = dbUser.UserId;
            email = dbUser.Email;
            name = dbUser.FullName ?? dbUser.Email;
        }

        // บันทึก Session จำเป็นสำหรับใช้ตรวจสอบทุกหน้า
        HttpContext.Session.SetInt32("UserId", mockUserId);
        HttpContext.Session.SetString("Role", normalizedRole);
        HttpContext.Session.SetString("UserEmail", email);
        HttpContext.Session.SetString("UserName", name);

        TempData["SuccessMessage"] = $"เข้าสู่ระบบด่วนในบทบาท {normalizedRole} สำเร็จ";
        return RedirectToAction(targetAction, targetController);
    }

    public IActionResult Logout()
    {
        // WHY: ล้าง Session ทั้งหมดเพื่อตัดการเชื่อมต่อและคืนสถานะเป็นผู้เยี่ยมชม (Guest)
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }
}
