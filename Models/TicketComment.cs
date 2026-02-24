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

        public string Initials
        {
            get
            {
                if (string.IsNullOrWhiteSpace(CreatedByName)) return "U";
                var parts = CreatedByName.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length >= 2)
                    return (parts[0][0].ToString() + parts[^1][0].ToString()).ToUpper();
                return CreatedByName.Substring(0, 1).ToUpper();
            }
        }
    }
}

