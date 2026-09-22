using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class AccountController : Controller
{

    public IActionResult Login()
    {
        return View();
    }

    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(RegisterViewModel data)
    {
        var Data = new RegisterViewModel();
        return RedirectToAction("Login","Account", Data);
    }

    public IActionResult ResetPassword()
    {
        return View();
    }

    public IActionResult Profile()
    {
        return View();
    }

    public IActionResult Lab5()
    {
        var Data = new Lab5ViewModel();
        Data.UserId = "linganguliguliguli wacha lingangu lingangu";
        Data.Password = "You're phon ringing!";
        Data.Address = "WAnkanda";
        Data.Age = 60;
        Data.Height = 1230;
        Data.Weight = 1567;

        return View(Data);
    }

    public IActionResult Lab5_2()
    {
    //     var Data = new List<Lab5ViewModel>
    //     {
    //         new Lab5ViewModel{UserId = "h1", Password = "B"},
    //         new Lab5ViewModel{UserId = "h2", Password = "D"},
    //         new Lab5ViewModel{UserId = "h3", Password = "D"},
    //     };
    // return View(Data);
    return View();
    }

    public IActionResult Lab5_3()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Lab5_3(Lab5ViewModel data)
    {
        string A, B, C;
        A = data.Name;
        B = data.Password;
        ViewBag.Name = data.Name;
        ViewBag.Password = data.Password;

        return View(data);
    }
}
