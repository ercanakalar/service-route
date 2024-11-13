using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Core.Models.User;

namespace Backend.Core.Models.Auth
{
    public class AuthDto
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        public string Username { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "Email is required.")]
        public string Email { get; set; } = string.Empty;

        public List<string> Roles { get; set; } = new List<string> { "User" };

        [MinLength(8, ErrorMessage = "Password must be at least 8 characters long.")]
        [Required(ErrorMessage = "Password is required.")]
        public string Password { get; set; } = string.Empty;

        [NotMapped]
        [Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;

        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }

        public Backend.Core.Models.User.UserDto Users { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
