using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Core.Models.Company;

namespace Backend.Core.Models.Map
{
    public class Waypoints
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public int Latitude { get; set; }

        [Required]
        public int Longitude { get; set; }

        [Required]
        public int Order { get; set; }

        public int CompanyId { get; set; }
        public Backend.Core.Models.Company.Company Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
