using System.ComponentModel.DataAnnotations;

namespace KanbanTracker.Models
{
    public class CommentViewModel
    {
        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "ProjectId")]
        public string ProjectId { get; set; }

        [Required]
        [Display(Name = "ElementId")]
        public string ElementId { get; set; }

        [Required]
        [Display(Name = "ElementType")]
        public string ElementType { get; set; }
    }
}