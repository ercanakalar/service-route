using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Auth;
using Backend.Core.Services;
using Backend.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> CreateUser([FromBody] SignupRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = await _authService.CreateUser(request);
            if (newUser.IsSuccess == false)
            {
                return BadRequest(newUser);
            }

            Response.Cookies.Append("auth", newUser.Token, new CookieOptions { HttpOnly = true });
            Response.Headers.Append("Authorization", $"Bearer {newUser.Token}");

            return Ok(newUser);
        }

        [HttpPost("signin")]
        public async Task<IActionResult> Signin([FromBody] SigninRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.Signin(request);
            if (user.IsSuccess == false)
            {
                return BadRequest(user);
            }

            Response.Cookies.Append("auth", user.Token, new CookieOptions { HttpOnly = true });
            Response.Headers.Append("Authorization", $"Bearer {user.Token}");

            return Ok(user);
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SigOut()
        {
            if (Request.Cookies.ContainsKey("auth"))
            {
                Response.Cookies.Delete("auth");
            }

            return Ok(new { message = "Signed out successfully." });
        }

        [HttpGet("users")]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _authService.GetUsers();
            return Ok(users);
        }

        [HttpGet("users/{id}")]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _authService.GetUserById(id);
            return Ok(user);
        }

        [HttpPut("users")]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _authService.UpdateUser(request);
            if (user.IsSuccess == false)
            {
                return BadRequest(user);
            }

            return Ok(user);
        }

        [HttpPut("users/{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateTheUser([FromBody] UpdateUserRequest request, int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var currentUserId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var currentUserRoles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToList();

            if (currentUserId != id && !currentUserRoles.Contains("Admin"))
            {
                return new ObjectResult(
                    new
                    {
                        IsSuccess = false,
                        ErrorMessage = "You are not authorized to update this user",
                        StatusCode = 403,
                    }
                );
            }

            var user = await _authService.UpdateTheUser(request, id);
            if (user.IsSuccess == false)
            {
                return BadRequest(user);
            }

            return Ok(user);
        }
    }
}
