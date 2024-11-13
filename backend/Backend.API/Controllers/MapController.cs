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
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Controllers
{
    [Route("api/map")]
    [ApiController]
    public class MapController : ControllerBase
    {
        private readonly IMapService _mapService;

        public MapController(IMapService mapService)
        {
            _mapService = mapService;
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

            if (waypoints == null)
            {
                return BadRequest(waypoints);
            }

            return Ok(waypoints);
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
