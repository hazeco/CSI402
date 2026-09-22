using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Khetsophon_CSI402_8456.Models;
using Microsoft.VisualBasic;
using Khetsophon_CSI402_8456.ViewModels;

namespace Khetsophon_CSI402_8456.Controllers;

public class StaffController : Controller
{
    [HttpPost]
    public IActionResult CounterPreorder(CounterPreorderViewModel data)
    {
        return RedirectToAction("Index", "Staff", data);
    }
    public IActionResult Index(CounterPreorderViewModel data)
    {
        ViewBag.ReservationNumber = data.ReservationNumber;
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
