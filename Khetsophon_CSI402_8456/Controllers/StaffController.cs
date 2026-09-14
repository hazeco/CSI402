using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;

namespace Khetsophon_CSI402_8456.Controllers;

public class StaffController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult ScanReceive()
    {
        return View();
    }

    public IActionResult CounterPreorder()
    {
        return View();
    }
}
