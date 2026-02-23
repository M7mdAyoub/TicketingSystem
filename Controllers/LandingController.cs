using Microsoft.AspNetCore.Mvc;

namespace HelpdeskApp.Controllers
{
    /// <summary>
    /// Public landing page — no login required.
    /// </summary>
    public class LandingController : Controller
    {
        public IActionResult Index()
        {
            // If already logged in, go to dashboard
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }
    }
}
