using System;
using System.ComponentModel.DataAnnotations;

namespace KanbanTracker.Models
{
    public class ProjectViewModel 
    {
        [Required]
        [Display(Name = "Title")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "Owner")]
        public string Owner { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Due")]
        public DateTime DueDate { get; set; }
    }
}