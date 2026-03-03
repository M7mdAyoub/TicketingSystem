using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using HelpdeskApp.Data;
using HelpdeskApp.Models;
using System.Collections.Generic;
using System;

namespace HelpdeskApp.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var model = new DashboardViewModel();

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
                            model.TotalTickets = reader.IsDBNull(0) ? 0 : reader.GetInt32(0);
                            model.OpenCount = reader.IsDBNull(1) ? 0 : reader.GetInt32(1);
                            model.InProgressCount = reader.IsDBNull(2) ? 0 : reader.GetInt32(2);
                            model.ClosedCount = reader.IsDBNull(3) ? 0 : reader.GetInt32(3);
                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Users", conn))
                    model.TotalUsers = (int)cmd.ExecuteScalar();

                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM Categories WHERE IsActive = 1", conn))
                    model.TotalCategories = (int)cmd.ExecuteScalar();

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
                            var createdDate = reader.GetDateTime(3);
                            var fullName = reader.GetString(reader.GetOrdinal("FullName"));
                            var status = reader.GetString(reader.GetOrdinal("Status"));
                            
                            var item = new RecentActivityItem
                            {
                                Id = reader.GetInt32(0),
                                Title = reader.GetString(1),
                                Status = status,
                                CreatedDate = createdDate,
                                CreatedByName = fullName,
                                Initials = GetInitials(fullName),
                                DotClass = status == "Closed" ? "dot-closed" : status == "InProgress" ? "dot-inprogress" : "dot-open",
                                StatusBadgeClass = status == "Open" ? "badge-open" : status == "InProgress" ? "badge-inprogress" : "badge-resolved",
                                StatusTextEn = status == "InProgress" ? "In Progress" : status,
                                StatusTextAr = status == "Open" ? "مفتوح" : status == "InProgress" ? "قيد التنفيذ" : "مغلق"
                            };
                            
                            // Calculate TimeAgo
                            var timeAgo = DateTime.Now - createdDate;
                            if (timeAgo.TotalMinutes < 60)
                            {
                                item.TimeAgoEn = $"{(int)timeAgo.TotalMinutes}m ago";
                                item.TimeAgoAr = $"منذ {(int)timeAgo.TotalMinutes} دقيقة";
                            }
                            else if (timeAgo.TotalHours < 24)
                            {
                                item.TimeAgoEn = $"{(int)timeAgo.TotalHours}h ago";
                                item.TimeAgoAr = $"منذ {(int)timeAgo.TotalHours} ساعة";
                            }
                            else
                            {
                                item.TimeAgoEn = $"{(int)timeAgo.TotalDays}d ago";
                                item.TimeAgoAr = $"منذ {(int)timeAgo.TotalDays} يوم";
                            }
                            
                            model.RecentActivity.Add(item);
                        }
                    }
                }
            }

            if (model.TotalTickets > 0)
            {
                model.OpenPct = Math.Round((double)model.OpenCount / model.TotalTickets * 100, 1);
                model.InProgressPct = Math.Round((double)model.InProgressCount / model.TotalTickets * 100, 1);
                model.ClosedPct = Math.Round((double)model.ClosedCount / model.TotalTickets * 100, 1);

                model.ClosedArc = model.Circumference * model.ClosedPct / 100;
                model.InProgressArc = model.Circumference * model.InProgressPct / 100;
                model.OpenArc = model.Circumference * model.OpenPct / 100;

                model.ClosedOffset = 0;
                model.InProgressOffset = -model.ClosedArc;
                model.OpenOffset = -(model.ClosedArc + model.InProgressArc);
            }

            return View(model);
        }

        private string GetInitials(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return "U";
            var parts = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length >= 2)
                return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
            return fullName.Substring(0, 1).ToUpper();
        }
    }
}

