using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Core.Models.Company;

namespace Backend.Core.Models.Map
{
    public class WaypointsDto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Order { get; set; }

        public int CompanyId { get; set; }
        public Backend.Core.Models.Company.CompanyDto Company { get; set; }

        [Required]
        public int PathId { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public string Address { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
