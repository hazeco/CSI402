using Microsoft.AspNetCore.Mvc;

namespace Khetsophon_CSI402_8456.Controllers;

public class AccountantController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Ledger()
    {
        return View();
    }

    public IActionResult Receipts()
    {
        return View();
    }
}
