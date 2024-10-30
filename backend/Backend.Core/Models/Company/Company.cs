using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Models.User;

namespace Backend.Core.Models.Company
{
    public class Company
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Company name is required.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company address is required.")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company country is required.")]
        public string Country { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company city is required.")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company state is required.")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company zip code is required.")]
        public string Zip { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company phone number is required.")]
        public string Phone { get; set; } = string.Empty;

        [Required(ErrorMessage = "Company email is required.")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        public string Email { get; set; } = string.Empty;

        public string? Website { get; set; }

        public string? Logo { get; set; }

        public string? Notes { get; set; }

        public List<Backend.Core.Models.User.User> User { get; set; } =
            new List<Backend.Core.Models.User.User>();
    }
}
