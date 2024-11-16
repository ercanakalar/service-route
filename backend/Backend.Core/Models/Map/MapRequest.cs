using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Core.Models.Map
{
    public class MapRequest
    {
        [Required]
        public string Address { get; set; }

        [Required]
        public double Latitude { get; set; }

        [Required]
        public double Longitude { get; set; }

        [Required]
        public int Order { get; set; }

        public int CompanyId { get; set; }

        [Required]
        public int PathId { get; set; }
    }
}
