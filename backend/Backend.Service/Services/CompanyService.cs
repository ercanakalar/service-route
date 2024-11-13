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
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
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

        public async Task<CompanyResponse> UpdateCompany(Company request)
        {
            var existingCompany = await _unitOfWork.Companies.GetByEmailAsync(request.Email);

            if (existingCompany == null)
            {
                return new CompanyResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company does not exist",
                    StatusCode = 400,
                };
            }

            var isThereEmail = await _unitOfWork.Companies.GetByEmailAsync(request.Email);
            if (isThereEmail != null && isThereEmail.Id != existingCompany.Id)
            {
                return new CompanyResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company with this email already exists",
                    StatusCode = 409,
                };
            }

            var isThereName = await _unitOfWork.Companies.GetByNameAsync(request.Name);
            if (isThereName != null && isThereName.Id != existingCompany.Id)
            {
                return new CompanyResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company with this name already exists",
                    StatusCode = 409,
                };
            }

            existingCompany.Name = request.Name ?? existingCompany.Name;
            existingCompany.Address = request.Address ?? existingCompany.Address;
            existingCompany.Country = request.Country ?? existingCompany.Country;
            existingCompany.City = request.City ?? existingCompany.City;
            existingCompany.State = request.State ?? existingCompany.State;
            existingCompany.Zip = request.Zip ?? existingCompany.Zip;
            existingCompany.Phone = request.Phone ?? existingCompany.Phone;
            existingCompany.Email = request.Email ?? existingCompany.Email;
            existingCompany.Website = request.Website ?? existingCompany.Website;
            existingCompany.Logo = request.Logo ?? existingCompany.Logo;
            existingCompany.Notes = request.Notes ?? existingCompany.Notes;
            existingCompany.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Companies.Update(existingCompany);
            await _unitOfWork.CommitAsync();

            return new CompanyResponse
            {
                IsSuccess = true,
                ErrorMessage = null,
                StatusCode = 200,
                Company = existingCompany,
            };
        }
    }
}
