using Backend.Core.Models.Company;

namespace Backend.Core.Services
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateCompany(Company company);
        Task<CompanyResponse> UpdateCompany(Company company);
    }
}
