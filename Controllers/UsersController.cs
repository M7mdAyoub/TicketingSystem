using Microsoft.Data.Sqlite;
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

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, FullName, Email, IsActive, CreatedDate FROM Users ORDER BY CreatedDate DESC";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        users.Add(new User
                        {
                            Id = (int)reader.GetInt64(reader.GetOrdinal("Id")),
                            FullName = reader.GetString(reader.GetOrdinal("FullName")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            IsActive = reader.GetInt64(reader.GetOrdinal("IsActive")) == 1,
                            CreatedDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedDate")))
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

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (var checkCmd = new SqliteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", user.Email);
                    long count = (long)checkCmd.ExecuteScalar()!;
                    if (count > 0)
                    {
                        ModelState.AddModelError("Email", "This email is already registered.");
                        return View(user);
                    }
                }

                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, IsActive, CreatedDate)
                                       VALUES (@FullName, @Email, @PasswordHash, @IsActive, datetime('now'))";

                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive ? 1 : 0);
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

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email";
                using (var checkCmd = new SqliteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                    long count = (long)checkCmd.ExecuteScalar()!;
                    if (count > 0)
                        return Json(new { success = false, error = "This email is already registered." });
                }

                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, IsActive, CreatedDate)
                                       VALUES (@FullName, @Email, @PasswordHash, @IsActive, datetime('now'))";

                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@PasswordHash", password);
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }

                using (var idCmd = new SqliteCommand("SELECT last_insert_rowid()", conn))
                {
                    long newId = (long)idCmd.ExecuteScalar()!;

                    var parts = fullName.Trim().Split(' ');
                    var initials = parts.Length >= 2
                        ? parts[0].Substring(0, 1).ToUpper() + parts[^1].Substring(0, 1).ToUpper()
                        : fullName.Trim().Substring(0, 1).ToUpper();

                    return Json(new
                    {
                        success = true,
                        id = (int)newId,
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

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email = @Email AND Id != @Id";
                using (var checkCmd = new SqliteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", email.Trim());
                    checkCmd.Parameters.AddWithValue("@Id", id);
                    if ((long)checkCmd.ExecuteScalar()! > 0)
                        return Json(new { success = false, error = "This email is already registered." });
                }

                string updateQuery = @"UPDATE Users SET FullName = @FullName, Email = @Email, IsActive = @IsActive WHERE Id = @Id";

                using (var cmd = new SqliteCommand(updateQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", fullName.Trim());
                    cmd.Parameters.AddWithValue("@Email", email.Trim());
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
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
        public IActionResult ToggleActive(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Users SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END WHERE Id = @Id";
                using (var cmd = new SqliteCommand(query, conn))
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
