namespace HelpdeskApp.Models
{
    /// <summary>
    /// ViewModel for the Tickets List page.
    /// Includes search, filter, and pagination properties.
    /// </summary>
    public class TicketListViewModel
    {
        public List<Ticket> Tickets { get; set; } = new();

        // Search & Filter
        public string? SearchTerm { get; set; }
        public int? FilterCategoryId { get; set; }
        public string? FilterStatus { get; set; }

        // Pagination
        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;

        // Dropdown data
        public List<Category> Categories { get; set; } = new();
    }
}
