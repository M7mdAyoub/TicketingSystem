using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Data;
using HelpdeskApp.Models;

namespace HelpdeskApp.Controllers
{
    /// <summary>
    /// Handles user login and logout using cookie authentication.
    /// </summary>
    public class AccountController : Controller
    {
        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            // If already logged in, redirect to Home
            if (User.Identity != null && User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");

            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Query the database for a user with matching email and password
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT Id, FullName, Email, IsActive 
                                 FROM Users 
                                 WHERE Email = @Email AND PasswordHash = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", model.Email);
                    cmd.Parameters.AddWithValue("@Password", model.Password); // Plain text comparison for simplicity

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool isActive = reader.GetBoolean(reader.GetOrdinal("IsActive"));

                            // Business Rule: Inactive users cannot log in
                            if (!isActive)
                            {
                                ModelState.AddModelError("", "Your account is deactivated. Contact an administrator.");
                                return View(model);
                            }

                            int userId = reader.GetInt32(reader.GetOrdinal("Id"));
                            string fullName = reader.GetString(reader.GetOrdinal("FullName"));
                            string email = reader.GetString(reader.GetOrdinal("Email"));

                            // Create authentication claims
                            var claims = new List<Claim>
                            {
                                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                                new Claim(ClaimTypes.Name, fullName),
                                new Claim(ClaimTypes.Email, email)
                            };

                            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                            var principal = new ClaimsPrincipal(identity);

                            // Sign in the user
                            await HttpContext.SignInAsync(
                                CookieAuthenticationDefaults.AuthenticationScheme,
                                principal);

                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid email or password.");
                            return View(model);
                        }
                    }
                }
            }
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }
    }
}
