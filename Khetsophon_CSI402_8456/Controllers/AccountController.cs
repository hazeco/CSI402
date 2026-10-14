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
                newUser.AvatarUrl = "";
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
                HttpContext.Session.SetString("Role", user.RoleName);
                HttpContext.Session.SetString("UserEmail", user.Email);
                
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

    public IActionResult Logout()
    {
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
