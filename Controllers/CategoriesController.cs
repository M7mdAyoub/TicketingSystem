using Microsoft.Data.Sqlite;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HelpdeskApp.Data;
using HelpdeskApp.Models;

namespace HelpdeskApp.Controllers
{
    [Authorize]
    public class CategoriesController : Controller
    {
        public IActionResult Index()
        {
            var categories = new List<Category>();

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, Name, IsActive, CreatedDate FROM Categories ORDER BY Id";

                using (var cmd = new SqliteCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = (int)reader.GetInt64(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            IsActive = reader.GetInt64(reader.GetOrdinal("IsActive")) == 1,
                            CreatedDate = DateTime.Parse(reader.GetString(reader.GetOrdinal("CreatedDate")))
                        });
                    }
                }
            }

            return View(categories);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)
        {
            if (!ModelState.IsValid)
                return View(category);

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Categories WHERE Name = @Name";
                using (var checkCmd = new SqliteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Name", category.Name);
                    long count = (long)checkCmd.ExecuteScalar()!;
                    if (count > 0)
                    {
                        ModelState.AddModelError("Name", "A category with this name already exists.");
                        return View(category);
                    }
                }

                string insertQuery = @"INSERT INTO Categories (Name, IsActive, CreatedDate)
                                       VALUES (@Name, @IsActive, datetime('now'))";

                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.Parameters.AddWithValue("@IsActive", category.IsActive ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Category created successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateAjax([FromForm] string name, [FromForm] bool isActive)
        {
            if (string.IsNullOrWhiteSpace(name))
                return Json(new { success = false, error = "Category name is required." });

            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string checkQuery = "SELECT COUNT(*) FROM Categories WHERE Name = @Name";
                using (var checkCmd = new SqliteCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Name", name.Trim());
                    long count = (long)checkCmd.ExecuteScalar()!;
                    if (count > 0)
                        return Json(new { success = false, error = "A category with this name already exists." });
                }

                string insertQuery = @"INSERT INTO Categories (Name, IsActive, CreatedDate)
                                       VALUES (@Name, @IsActive, datetime('now'))";

                using (var cmd = new SqliteCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", name.Trim());
                    cmd.Parameters.AddWithValue("@IsActive", isActive ? 1 : 0);
                    cmd.ExecuteNonQuery();
                }

                // Get the inserted row
                using (var idCmd = new SqliteCommand("SELECT last_insert_rowid()", conn))
                {
                    long newId = (long)idCmd.ExecuteScalar()!;
                    return Json(new
                    {
                        success = true,
                        id = (int)newId,
                        name = name.Trim(),
                        isActive = isActive,
                        createdDate = DateTime.UtcNow.ToString("MMM dd, yyyy")
                    });
                }
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ToggleActive(int id)
        {
            using (var conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Categories SET IsActive = CASE WHEN IsActive = 1 THEN 0 ELSE 1 END WHERE Id = @Id";
                using (var cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Category status updated.";
            return RedirectToAction("Index");
        }
    }
}
