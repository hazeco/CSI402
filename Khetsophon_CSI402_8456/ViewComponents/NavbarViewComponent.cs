using Microsoft.AspNetCore.Mvc;

namespace Khetsophon_CSI402_8456.Controllers;

public class  NavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string role = "guest")
    {
        // role = "guest" -> Default.cshtml
        // role = "staff" -> Staff.cshtml
        string viewName = role == "guest" ? "Default" : role;
        return View(viewName, model: role);
    }
}