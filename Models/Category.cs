using System.ComponentModel.DataAnnotations;

namespace HelpdeskApp.Models
{
    /// <summary>
    /// Represents a ticket category (e.g., Software, Hardware, Network).
    /// Maps to the Categories table in the database.
    /// </summary>
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category Name is required.")]
        [StringLength(100, ErrorMessage = "Category Name cannot exceed 100 characters.")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}
