using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Data;
using HelpdeskApp.Models;

namespace HelpdeskApp.Controllers
{
    [Authorize]
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            var users = new List<User>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, FullName, Email, IsActive, CreatedDate FROM Users ORDER BY CreatedDate DESC";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.GetBoolean(reader.GetOrdinal("IsActive")),
                            CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate"))
                        });
                    }
                }
            }

            return View(users);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", user.Email);
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(user);
                    }
                }

                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, IsActive, CreatedDate)
                                       VALUES (@FullName, @Email, @PasswordHash, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "User created successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax([FromForm] string fullName, [FromForm] string email, [FromForm] string password, [FromForm] bool isActive)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return Json(new { success = false, error = "Full name is required." });
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, error = "Email is required." });
            if (string.IsNullOrWhiteSpace(password))
                return Json(new { success = false, error = "Password is required." });
            if (password.Length < 8)
                return Json(new { success = false, error = "Password must be at least 8 characters." });

            var regex = new System.Text.RegularExpressions.Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&#])[A-Za-z\d@$!%*?&#]{8,}$");
            if (!regex.IsMatch(password))
                return Json(new { success = false, error = "Password must have uppercase, lowercase, number, and special character." });

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                    int count = (int)checkCmd.ExecuteScalar();
                    if (count > 0)
                        return Json(new { success = false, error = "This email is already registered." });
                }

                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, IsActive, CreatedDate)
                                       OUTPUT INSERTED.Id
                                       VALUES (@FullName, @Email, @PasswordHash, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    int newId = (int)cmd.ExecuteScalar();

                    var parts = fullName.Trim().Split(' ');
                    var initials = parts.Length >= 2
                        ? parts[0].Substring(0, 1).ToUpper() + parts[^1].Substring(0, 1).ToUpper()
                        : fullName.Trim().Substring(0, 1).ToUpper();

                    return Json(new
                    {
                        success = true,
                        id = newId,
                        fullName = fullName.Trim(),
                        email = email.Trim(),
                        isActive = isActive,
                        initials = initials
                    });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditAjax([FromForm] int id, [FromForm] string fullName, [FromForm] string email, [FromForm] bool isActive)
        {
            if (string.IsNullOrWhiteSpace(fullName))
                return Json(new { success = false, error = "Full name is required." });
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, error = "Email is required." });

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Id != @Id";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                    checkCmd.Parameters.AddWithValue("@Id", id);
                    if ((int)checkCmd.ExecuteScalar() > 0)
                        return Json(new { success = false, error = "This email is heavily already registered." });
                }

                string updateQuery = @"UPDATE Users SET FullName = @FullName, Email = @Email, IsActive = @IsActive WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@IsActive", isActive);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
            
            var parts = fullName.Trim().Split(' ');
            var initials = parts.Length >= 2
                ? parts[0].Substring(0, 1).ToUpper() + parts[^1].Substring(0, 1).ToUpper()
                : fullName.Trim().Substring(0, 1).ToUpper();

            return Json(new { success = true, id = id, fullName = fullName.Trim(), email = email.Trim(), isActive = isActive, initials = initials });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendEmailAjax([FromForm] string email, [FromForm] string title, [FromForm] string subject)
        {
            if (string.IsNullOrWhiteSpace(email))
                return Json(new { success = false, error = "Email is required." });
            if (string.IsNullOrWhiteSpace(title))
                return Json(new { success = false, error = "Title is required." });
            if (string.IsNullOrWhiteSpace(subject))
                return Json(new { success = false, error = "Subject is required." });

            try
            {
                // Here we simulate an immediate email send without popping open an email provider.
                // In production, configure an SmtpClient or SendGrid API here.
                System.Threading.Thread.Sleep(800);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Users SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "User status updated.";
            return RedirectToAction("Index");
        }
    }
}
