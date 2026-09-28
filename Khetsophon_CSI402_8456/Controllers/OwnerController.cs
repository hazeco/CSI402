using Microsoft.AspNetCore.Mvc;

namespace Khetsophon_CSI402_8456.Controllers;

public class OwnerController : Controller
{
    public IActionResult Index()
    {
        return RedirectToAction("Promotions");
    }

    public IActionResult Promotions()
    {
        return View();
    }

    public IActionResult AddPromotion()
    {
        return View();
    }

    public IActionResult EditPromotion()
    {
        return View();
    }

    public IActionResult MonthlyReport()
    {
        return View();
    }
}
