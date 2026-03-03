using System;
using System.Collections.Generic;

namespace HelpdeskApp.Models
{
    public class DashboardViewModel
    {
        public int TotalTickets { get; set; }
        public int OpenCount { get; set; }
        public int InProgressCount { get; set; }
        public int ClosedCount { get; set; }
        public int TotalUsers { get; set; }
        public int TotalCategories { get; set; }

        public double OpenPct { get; set; }
        public double InProgressPct { get; set; }
        public double ClosedPct { get; set; }

        public double Circumference { get; set; } = 377;
        public double ClosedArc { get; set; }
        public double InProgressArc { get; set; }
        public double OpenArc { get; set; }
        public double ClosedOffset { get; set; }
        public double InProgressOffset { get; set; }
        public double OpenOffset { get; set; }

        public List<RecentActivityItem> RecentActivity { get; set; } = new();
    }

    public class RecentActivityItem
    {
        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime CreatedDate { get; set; }
        public string CreatedByName { get; set; } = "";
        
        public string TimeAgoEn { get; set; } = "";
        public string TimeAgoAr { get; set; } = "";
        public string DotClass { get; set; } = "";
        public string StatusBadgeClass { get; set; } = "";
        public string StatusTextEn { get; set; } = "";
        public string StatusTextAr { get; set; } = "";
        
        public string Initials { get; set; } = "";
    }
}
