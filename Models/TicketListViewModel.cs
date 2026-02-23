namespace HelpdeskApp.Models
{
    public class TicketListViewModel
    {
        public List<Ticket> Tickets { get; set; } = new();

        public string? SearchTerm { get; set; }
        public int? FilterCategoryId { get; set; }
        public string? FilterStatus { get; set; }

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 5;

        public List<Category> Categories { get; set; } = new();
    }
}
