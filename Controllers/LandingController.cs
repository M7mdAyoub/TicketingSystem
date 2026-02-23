using Microsoft.AspNetCore.Mvc;

namespace HelpdeskApp.Controllers
{
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
