using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Data;
using HelpdeskApp.Models;

namespace HelpdeskApp.Controllers
{
    /// <summary>
    /// Manages Users: List and Create.
    /// All database access uses ADO.NET with parameterized queries.
    /// </summary>
    [Authorize]
    public class UsersController : Controller
    {
        // GET: /Users
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

        // GET: /Users/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /Users/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                // Business Rule: Check if email already exists (must be unique)
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

                // Insert the new user
                string insertQuery = @"INSERT INTO Users (FullName, Email, PasswordHash, IsActive, CreatedDate)
                                       VALUES (@FullName, @Email, @PasswordHash, @IsActive, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@FullName", user.FullName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@PasswordHash", user.PasswordHash); // Stored as plain text for simplicity
                    cmd.Parameters.AddWithValue("@IsActive", user.IsActive);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "User created successfully.";
            return RedirectToAction("Index");
        }

        // POST: /Users/ToggleActive/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                // Toggle IsActive flag
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
