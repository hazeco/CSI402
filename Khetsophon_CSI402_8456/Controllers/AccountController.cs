using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class AccountController : Controller
{
    public IActionResult Register()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Register(RegisterViewModel data)
    {
        //send data to login page by viewbag
        return RedirectToAction("Login","Account", data);
    }
    public IActionResult Login(RegisterViewModel data)
    {
        // recive data from register page by viewbag
        ViewBag.Email = data.Email;
        ViewBag.Phone = data.Phone;
        ViewBag.Password = data.Password;
        return View();
    }

    [HttpPost]
    public IActionResult Login(LoginViewModel data) {
        //send data to home page by viewbag
        return RedirectToAction("Index", "Home", data);
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
