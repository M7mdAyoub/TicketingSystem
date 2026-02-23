using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    /// <summary>
    /// Represents a helpdesk ticket.
    /// Maps to the Tickets table in the database.
    /// </summary>
    public class Ticket
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Category is required.")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        /// <summary>
        /// The user who created this ticket (FK to Users.Id).
        /// </summary>
        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Open";

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        /// <summary>
        /// Soft delete flag. True means the ticket is logically deleted.
        /// </summary>
        public bool IsDeleted { get; set; } = false;

        // --- Navigation / display properties (not mapped to DB columns directly) ---

        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Created By")]
        public string? CreatedByName { get; set; }

        /// <summary>
        /// Comments associated with this ticket (loaded on Details page).
        /// </summary>
        public List<TicketComment>? Comments { get; set; }
    }
}
