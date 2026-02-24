using Microsoft.AspNetCore.Mvc;

namespace HelpdeskApp.Controllers
{
    public class LanguageController : Controller
    {
        [HttpGet]
        public IActionResult Switch(string lang, string returnUrl)
        {
            if (lang == "ar" || lang == "en")
            {
                Response.Cookies.Append("lang", lang, new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                });
            }

            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Landing");
        }
    }
}
