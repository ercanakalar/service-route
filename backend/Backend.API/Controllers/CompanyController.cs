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
        private readonly IAuthService _authService;
        private readonly ICompanyService _companyService;

        public CompanyController(IAuthService authService, ICompanyService companyService)
        {
            _authService = authService;
            _companyService = companyService;
        }

        [HttpPost]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> CreateCompany([FromBody] CompanyDto company)
        {
            var createdCompany = await _companyService.CreateCompany(company);

            return Ok(createdCompany);
        }

        [HttpPut]
        [Authorize(Policy = "ShouldBeAdminOrManager")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDto company)
        {
            var updatedCompany = await _companyService.UpdateCompany(company);

            return Ok(updatedCompany);
        }
    }
}
