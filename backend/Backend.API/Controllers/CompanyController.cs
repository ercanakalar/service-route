using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Auth;
using Backend.Core.Models.Company;
using Backend.Core.Services;
using Backend.Service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.API.Controllers
{
    [Route("api/company")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICompanyService _companyService;

        public CompanyController(IUserService userService, ICompanyService companyService)
        {
            _userService = userService;
            _companyService = companyService;
        }

        [HttpPost]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> CreateCompany([FromBody] Company company)
        {
            var createdCompany = await _companyService.CreateCompany(company);

            return Ok(createdCompany);
        }
    }
}
