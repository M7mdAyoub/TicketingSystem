using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
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

        [Display(Name = "Created By")]
        public int CreatedBy { get; set; }

        [Required(ErrorMessage = "Status is required.")]
        [Display(Name = "Status")]
        public string Status { get; set; } = "Open";

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsDeleted { get; set; } = false;

        [Display(Name = "Category")]
        public string? CategoryName { get; set; }

        [Display(Name = "Created By")]
        public string? CreatedByName { get; set; }

        public List<TicketComment>? Comments { get; set; }

        public string CreatedByInitials
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

