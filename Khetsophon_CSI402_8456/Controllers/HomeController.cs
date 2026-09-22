using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using System.Security.Cryptography.X509Certificates;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class HomeController : Controller
{
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
