using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Company;
using Backend.Core.Models.Error;
using Backend.Core.Services;
using Backend.Core.UnitOfWorks;
using Backend.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service.Services
{
    public class CompanyService : ICompanyService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenGenerator _tokenService;
        private readonly PasswordManager _passwordManager;

        public CompanyService(
            IUnitOfWork unitOfWork,
            JwtTokenGenerator tokenService,
            PasswordManager passwordManager
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordManager = passwordManager;
        }

        public async Task<CompanyResponse> CreateCompany(Company request)
        {
            var existingCompany = await _unitOfWork.Companies.GetByEmailAsync(request.Email);

            if (existingCompany != null)
            {
                return new CompanyResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company already exists",
                    StatusCode = 400,
                };
            }

            var company = new Company
            {
                Name = request.Name,
                Address = request.Address,
                Country = request.Country,
                City = request.City,
                State = request.State,
                Zip = request.Zip,
                Phone = request.Phone,
                Email = request.Email,
                Website = request.Website,
                Logo = request.Logo,
                Notes = request.Notes,
            };

            await _unitOfWork.Companies.AddAsync(company);
            await _unitOfWork.CommitAsync();

            return new CompanyResponse
            {
                IsSuccess = true,
                ErrorMessage = null,
                StatusCode = 201,
                Company = company,
            };
        }
    }
}
