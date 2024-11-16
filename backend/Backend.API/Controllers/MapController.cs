using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Auth;
using Backend.Core.Models.Map;
using Backend.Core.Services;
using Backend.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Controllers
{
    [Route("api/map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IMapService _mapService;
        private readonly ExternalApiService _externalApiService;

        public MapController(
            IConfiguration configuration,
            IMapService mapService,
            ExternalApiService externalApiService
        )
        {
            _configuration = configuration;
            _mapService = mapService;
            _externalApiService = externalApiService;
        }

        [HttpPost]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> InsertWaypoints([FromBody] MapRequest request)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }
            var waypoint = await _mapService.InsertWaypoints(request, int.Parse(userId));
            if (!waypoint.IsSuccess)
            {
                return BadRequest(waypoint);
            }
            return Ok(waypoint);
        }

        [HttpGet]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> GetWaypointsAsAdmin()
        {
            var waypoints = await _mapService.GetWaypointsAsAdmin();

            if (waypoints == null || waypoints.Waypoints == null || !waypoints.Waypoints.Any())
            {
                return BadRequest("Waypoints data is empty or null.");
            }

            var sortedWaypoints = waypoints.Waypoints.OrderBy(wp => wp.Order).ToList();

            var origin = sortedWaypoints.First();
            var destination = sortedWaypoints.Last();

            var waypointsString = string.Join(
                "|",
                sortedWaypoints
                    .Skip(1)
                    .Take(sortedWaypoints.Count - 2)
                    .Select(wp => $"{wp.Latitude},{wp.Longitude}")
            );

            var apiKey = _configuration["Map:MapApiKey"];

            var url =
                $"https://maps.googleapis.com/maps/api/directions/json?origin={origin.Latitude},{origin.Longitude}&destination={destination.Latitude},{destination.Longitude}&waypoints={waypointsString}&key={apiKey}";

            var data = await _externalApiService.FetchDataAsync(url);

            return Ok(data);
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetWaypointsAsUser()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
            {
                return Unauthorized();
            }

            var waypoints = await _mapService.GetWaypointsAsUser(int.Parse(userId));

            if (waypoints == null)
            {
                return BadRequest(waypoints);
            }

            return Ok(waypoints);
        }
    }
}
