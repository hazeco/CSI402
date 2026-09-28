using Microsoft.AspNetCore.Mvc;

namespace Khetsophon_CSI402_8456.Controllers;

public class ITSupportController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Dashboard");
    }

    public IActionResult Dashboard()
    {
        return View();
    }

    public IActionResult Users()
    {
        return View();
    }

    public IActionResult Logs()
    {
        return View();
    }
}
