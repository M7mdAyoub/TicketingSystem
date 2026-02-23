using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    public class TicketComment
    {
        public int Id { get; set; }

        [Required]
        public int TicketId { get; set; }

        [Required(ErrorMessage = "Comment text is required.")]
        [Display(Name = "Comment")]
        public string CommentText { get; set; } = string.Empty;

        [Display(Name = "Commented By")]
        public int CreatedByU { get; set; }

        [Display(Name = "Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Commented By")]
        public string? CreatedByName { get; set; }
    }
}
