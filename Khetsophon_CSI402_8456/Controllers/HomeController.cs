using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class HomeController : Controller
{
    private readonly Csi402dbContext _db;
    public HomeController (Csi402dbContext db)
    {
        _db = db;
    }

    public IActionResult Lab9()
    {
        // Create

        return View();
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Lab9(Labstudent data)
    {
        if(ModelState.IsValid)
        {
            if(data.StdPassword != null)
            {
                var CheckID = (from a in _db.Labstudents where a.StdId == data.StdId select a).FirstOrDefault();
                //select * from Labstudents where StdId == "@StdId"
                if(CheckID == null)
                {
                    Labstudent s = new Labstudent();
                    //table2
                    s.StdId = data.StdId;
                    s.StdPassword = data.StdPassword;
                    s.StdName = data.StdName;
                    s.StdLastname = data.StdLastname;
                    _db.Add(s);
                    _db.SaveChanges();
                    return View(data);
                } else {
                    ViewBag.Status = "This ID is already exists";
                    return View();
                }
                
            } else
            {
                ViewBag.Status = "Please enter password";
                return View();
            }
        } else
        {
            return View();
        }
    }

    // Read
    public IActionResult Lab9_1()
    {
        // Read data from Labstudents table and return to view
        var data = _db.Labstudents.ToList();
        return View(data);
    }

    public IActionResult UserList()
    {
        var users = _db.Users.ToList();
        return View(users);
    }
    public IActionResult Index(LoginViewModel? data)
    {
        // receive data from login page by viewbag
        if (data != null)
        {
            ViewBag.Email = data.Email;
            ViewBag.Password = data.Password;
        }

        var heroProduct = _db.Products.FirstOrDefault(p => p.IsFeaturedHero)
                          ?? _db.Products.FirstOrDefault(p => !string.IsNullOrEmpty(p.Model3Dpath));

        if (heroProduct != null)
        {
            ViewBag.HeroModel3D = !string.IsNullOrEmpty(heroProduct.Model3Dpath) ? heroProduct.Model3Dpath : "~/media/models/naruto_kyubi_sage.glb";
            ViewBag.HeroName = heroProduct.Name;
            ViewBag.HeroPrice = $"฿{heroProduct.RetailPrice:N0}";
        }

        return View();
    }

    public IActionResult Collection()
    {
        var products = _db.Products.Where(p => p.IsActive == true || p.IsActive == null).ToList();
        return View(products);
    }

    public IActionResult Promotion()
    {
        var promotions = _db.Promotions.Where(p => p.IsActive == true || p.IsActive == null).ToList();
        return View(promotions);
    }

    public IActionResult Customer()
    {
        return View("Index");
    }

    public IActionResult Cart()
    {
        return View();
    }

    public IActionResult ProductDetail()
    {
        return View();
    }

    public IActionResult PreorderStatus()
    {
        // WHY: ดึงสถานะพรีออเดอร์ของสมาชิกปัจจุบันเพื่อนำมาแสดงผลแบบ Real-time ตามสถานะจริงใน DB
        var userId = HttpContext.Session.GetInt32("UserId");
        Preorder? preorder = null;

        if (userId.HasValue)
        {
            preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                    .ThenInclude(pi => pi.Product)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefault(p => p.UserId == userId.Value);
        }

        // หากยังไม่ได้ล็อกอิน ให้ดึงใบจองล่าสุดในระบบ หรือปล่อยให้ View ใช้ Fallback Mock
        if (preorder == null)
        {
            preorder = _db.Preorders
                .Include(p => p.Preorderitems)
                    .ThenInclude(pi => pi.Product)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefault();
        }

        return View(preorder);
    }

    public IActionResult Rewards()
    {
        var rewards = _db.Rewards.Where(r => r.IsAvailable == true || r.IsAvailable == null).ToList();
        return View(rewards);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
