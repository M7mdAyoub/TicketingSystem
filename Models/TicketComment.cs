using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    /// <summary>
    /// Represents a comment on a ticket.
    /// Maps to the TicketComments table in the database.
    /// </summary>
    public class TicketComment
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        [Display(Name = "Comment")]
        public string CommentText { get; set; } = string.Empty;

        /// <summary>
        /// The user who wrote this comment (FK to Users.Id).
        /// </summary>
        [Display(Name = "Commented By")]
        public int CreatedByU { get; set; }

        [Display(Name = "Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // --- Display property ---
        [Display(Name = "Commented By")]
        public string? CreatedByName { get; set; }
    }
}
