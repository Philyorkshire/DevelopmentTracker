using System.ComponentModel.DataAnnotations;

namespace KanbanTracker.Models
{
    public class StoryViewModel 
    {
        [Required]
        [Display(Name = "Id")]
        public string Id { get; set; }

        [Display(Name = "ProjectId")]
        public string ProjectId { get; set; }   

        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Assigned")]
        public string Assigned { get; set; }

        [Required]
        [Display(Name = "Status")]
        public string Status { get; set; } 
    }
}