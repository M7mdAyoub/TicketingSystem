using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using HelpdeskApp.Data;

namespace HelpdeskApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            int totalTickets = 0, openCount = 0, inProgressCount = 0, closedCount = 0;
            int totalUsers = 0, totalCategories = 0;
            var recentTickets = new List<dynamic>();

            using (SqlConnection conn = DbHelper.GetConnection())
            {
                conn.Open();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT 
                        COUNT(*) AS Total,
                        SUM(CASE WHEN Status = 'Open' THEN 1 ELSE 0 END) AS OpenCount,
                        SUM(CASE WHEN Status = 'InProgress' THEN 1 ELSE 0 END) AS InProgressCount,
                        SUM(CASE WHEN Status = 'Closed' THEN 1 ELSE 0 END) AS ClosedCount
                      FROM Tickets WHERE IsDeleted = 0", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            totalTickets = reader.GetInt32(0);
                            openCount = reader.GetInt32(1);
                            inProgressCount = reader.GetInt32(2);
                            closedCount = reader.GetInt32(3);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn))
                    totalUsers = (int)cmd.ExecuteScalar();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Categories WHERE IsActive = 1", conn))
                    totalCategories = (int)cmd.ExecuteScalar();

                using (SqlCommand cmd = new SqlCommand(
                    @"SELECT TOP 5 t.Id, t.Title, t.Status, t.CreatedDate, u.FullName
                      FROM Tickets t
                      INNER JOIN Users u ON t.CreatedBy = u.Id
                      WHERE t.IsDeleted = 0
                      ORDER BY t.CreatedDate DESC", conn))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recentTickets.Add(new
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Status = reader.GetString(2),
                                CreatedDate = reader.GetDateTime(3),
                                CreatedByName = reader.GetString(4)
                            });
                        }
                    }
                }
            }

            ViewBag.TotalTickets = totalTickets;
            ViewBag.OpenCount = openCount;
            ViewBag.InProgressCount = inProgressCount;
            ViewBag.ClosedCount = closedCount;
            ViewBag.TotalUsers = totalUsers;
            ViewBag.TotalCategories = totalCategories;
            ViewBag.RecentTickets = recentTickets;

            return View();
        }
    }
}
