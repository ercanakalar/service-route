using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Backend.Core.Models.Map;

namespace Backend.Core.Models.Map
{
    public class MapResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; } = 200;
        public List<Waypoints> Waypoints { get; set; } = new List<Waypoints>();
    }
}
