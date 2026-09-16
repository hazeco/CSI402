using Microsoft.AspNetCore.Mvc;

namespace Khetsophon_CSI402_8456.Controllers;

public class  NavbarViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(string role = "guest")
    {
        string viewName = (role?.ToLowerInvariant()) switch
        {
            "staff" => "Staff",
            "owner" => "Owner",
            "itsupport" or "it" => "ITSupport",
            "accountant" or "acc" => "Accountant",
            _ => "Default"
        };
        return View(viewName, model: role);
    }
}