using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;
using MongoDB.Driver;

namespace KanbanTracker.Models
{
    public static class UserDb
    {
        public static MongoCollection<IUser> Open()
        {
            var client = new MongoClient("mongodb://localhost");
            var server = client.GetServer();
            var db = server.GetDatabase("UserDb");
            return db.GetCollection<IUser>("Users");
        }
    }

    public class RegisterViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.Web.Mvc.Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }
}