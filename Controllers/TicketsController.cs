using Microsoft.Data.SqlClient;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using HelpdeskApp.Data;
using HelpdeskApp.Models;

namespace HelpdeskApp.Controllers
{
    [Authorize]
    public class TicketsController : Controller
    {
        private int GetCurrentUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim != null ? int.Parse(claim.Value) : 0;
        }

        public IActionResult Index(string? searchTerm, int? filterCategoryId, string? filterStatus, int page = 1)
        {
            int pageSize = 5;
            var model = new TicketListViewModel
            {
                SearchTerm = searchTerm,
                FilterCategoryId = filterCategoryId,
                FilterStatus = filterStatus,
                CurrentPage = page,
                PageSize = pageSize
            };

            model.Categories = GetAllActiveCategories();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string whereClause = "WHERE t.IsDeleted = 0";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    whereClause += " AND t.Title LIKE @SearchTerm";
                    parameters.Add(new SqlParameter("@SearchTerm", "%" + searchTerm + "%"));
                }

                if (filterCategoryId.HasValue && filterCategoryId.Value > 0)
                {
                    whereClause += " AND t.CategoryId = @CategoryId";
                    parameters.Add(new SqlParameter("@CategoryId", filterCategoryId.Value));
                }

                if (!string.IsNullOrWhiteSpace(filterStatus))
                {
                    whereClause += " AND t.Status = @Status";
                    parameters.Add(new SqlParameter("@Status", filterStatus));
                }

                string countQuery = $"SELECT COUNT(*) FROM Tickets t {whereClause}";
                using (SqlCommand countCmd = new SqlCommand(countQuery, conn))
                {
                    countCmd.Parameters.AddRange(parameters.ToArray());
                    int totalRecords = (int)countCmd.ExecuteScalar();
                    model.TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize);
                }

                string query = $@"
                    SELECT t.Id, t.Title, t.Description, t.CategoryId, t.CreatedBy,
                           t.Status, t.CreatedDate, c.Name AS CategoryName, u.FullName AS CreatedByName
                    FROM Tickets t
                    INNER JOIN Categories c ON t.CategoryId = c.Id
                    INNER JOIN Users u ON t.CreatedBy = u.Id
                    {whereClause}
                    ORDER BY t.CreatedDate DESC
                    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY";

                var queryParams = new List<SqlParameter>();
                if (!string.IsNullOrWhiteSpace(searchTerm))
                    queryParams.Add(new SqlParameter("@SearchTerm", "%" + searchTerm + "%"));
                if (filterCategoryId.HasValue && filterCategoryId.Value > 0)
                    queryParams.Add(new SqlParameter("@CategoryId", filterCategoryId.Value));
                if (!string.IsNullOrWhiteSpace(filterStatus))
                    queryParams.Add(new SqlParameter("@Status", filterStatus));

                queryParams.Add(new SqlParameter("@Offset", (page - 1) * pageSize));
                queryParams.Add(new SqlParameter("@PageSize", pageSize));

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(queryParams.ToArray());

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            model.Tickets.Add(new Ticket
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                CreatedByName = reader.GetString(reader.GetOrdinal("CreatedByName"))
                            });
                        }
                    }
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = GetAllActiveCategories();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Ticket ticket)
        {
            ModelState.Remove("CategoryName");
            ModelState.Remove("CreatedByName");
            ModelState.Remove("Comments");

            if (!ModelState.IsValid)
            {
                ViewBag.Categories = GetAllActiveCategories();
                return View(ticket);
            }

            ticket.CreatedBy = GetCurrentUserId();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = @"INSERT INTO Tickets (Title, Description, CategoryId, CreatedBy, Status, CreatedDate, IsDeleted)
                                 VALUES (@Title, @Description, @CategoryId, @CreatedBy, @Status, GETDATE(), 0)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", ticket.Title);
                    cmd.Parameters.AddWithValue("@Description", ticket.Description);
                    cmd.Parameters.AddWithValue("@CategoryId", ticket.CategoryId);
                    cmd.Parameters.AddWithValue("@CreatedBy", ticket.CreatedBy);
                    cmd.Parameters.AddWithValue("@Status", ticket.Status);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Ticket created successfully.";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Details(int id)
        {
            Ticket? ticket = null;

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = @"SELECT t.Id, t.Title, t.Description, t.CategoryId, t.CreatedBy,
                                        t.Status, t.CreatedDate, t.IsDeleted,
                                        c.Name AS CategoryName, u.FullName AS CreatedByName
                                 FROM Tickets t
                                 INNER JOIN Categories c ON t.CategoryId = c.Id
                                 INNER JOIN Users u ON t.CreatedBy = u.Id
                                 WHERE t.Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ticket = new Ticket
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Description = reader.GetString(reader.GetOrdinal("Description")),
                                CategoryId = reader.GetInt32(reader.GetOrdinal("CategoryId")),
                                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                                Status = reader.GetString(reader.GetOrdinal("Status")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                IsDeleted = reader.GetBoolean(reader.GetOrdinal("IsDeleted")),
                                CategoryName = reader.GetString(reader.GetOrdinal("CategoryName")),
                                CreatedByName = reader.GetString(reader.GetOrdinal("CreatedByName"))
                            };
                        }
                    }
                }

                if (ticket == null)
                    return NotFound();

                ticket.Comments = new List<TicketComment>();
                string commentsQuery = @"SELECT tc.Id, tc.TicketId, tc.CommentText, tc.CreatedByU, tc.CreatedDate,
                                                u.FullName AS CreatedByName
                                         FROM TicketComments tc
                                         INNER JOIN Users u ON tc.CreatedByU = u.Id
                                         WHERE tc.TicketId = @TicketId
                                         ORDER BY tc.CreatedDate ASC";

                using (SqlCommand commentsCmd = new SqlCommand(commentsQuery, conn))
                {
                    commentsCmd.Parameters.AddWithValue("@TicketId", id);

                    using (SqlDataReader reader = commentsCmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ticket.Comments.Add(new TicketComment
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                TicketId = reader.GetInt32(reader.GetOrdinal("TicketId")),
                                CommentText = reader.GetString(reader.GetOrdinal("CommentText")),
                                CreatedByU = reader.GetInt32(reader.GetOrdinal("CreatedByU")),
                                CreatedDate = reader.GetDateTime(reader.GetOrdinal("CreatedDate")),
                                CreatedByName = reader.GetString(reader.GetOrdinal("CreatedByName"))
                            });
                        }
                    }
                }
            }

            return View(ticket);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddComment(int ticketId, string commentText)
        {
            if (string.IsNullOrWhiteSpace(commentText))
            {
                TempData["Error"] = "Comment text cannot be empty.";
                return RedirectToAction("Details", new { id = ticketId });
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string statusQuery = "SELECT Status FROM Tickets WHERE Id = @Id";
                using (SqlCommand statusCmd = new SqlCommand(statusQuery, conn))
                {
                    statusCmd.Parameters.AddWithValue("@Id", ticketId);
                    string? status = statusCmd.ExecuteScalar()?.ToString();

                    if (status == "Closed")
                    {
                        TempData["Error"] = "Cannot add comments to a closed ticket.";
                        return RedirectToAction("Details", new { id = ticketId });
                    }
                }

                string query = @"INSERT INTO TicketComments (TicketId, CommentText, CreatedByU, CreatedDate)
                                 VALUES (@TicketId, @CommentText, @CreatedByU, GETDATE())";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TicketId", ticketId);
                    cmd.Parameters.AddWithValue("@CommentText", commentText);
                    cmd.Parameters.AddWithValue("@CreatedByU", GetCurrentUserId());
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Comment added successfully.";
            return RedirectToAction("Details", new { id = ticketId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SoftDelete(int id)
        {
            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Tickets SET IsDeleted = 1 WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Ticket deleted successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateStatus(int id, string status)
        {
            if (status != "Open" && status != "InProgress" && status != "Closed")
            {
                TempData["Error"] = "Invalid status value.";
                return RedirectToAction("Details", new { id });
            }

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                string query = "UPDATE Tickets SET Status = @Status WHERE Id = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Status", status);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }

            TempData["Success"] = "Ticket status updated.";
            return RedirectToAction("Details", new { id });
        }

        private List<Category> GetAllActiveCategories()
        {
            var categories = new List<Category>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();
                string query = "SELECT Id, Name FROM Categories WHERE IsActive = 1 ORDER BY Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        });
                    }
                }
            }

            return categories;
        }
    }
}
