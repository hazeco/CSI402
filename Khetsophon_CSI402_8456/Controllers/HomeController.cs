using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
    public IActionResult Index(LoginViewModel data)
    {
        // recive data from login page by viewbag
        ViewBag.Email = data.Email;
        ViewBag.Password = data.Password;
        return View();
    }

    public IActionResult Collection()
    {
        return View();
    }

    public IActionResult Promotion()
    {
        return View();
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
        return View();
    }

    public IActionResult Rewards()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
