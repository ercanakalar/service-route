using Backend.Core.Models.Company;

namespace Backend.Core.Services
{
    public interface ICompanyService
    {
        Task<CompanyResponse> CreateCompany(CompanyDto company);
        Task<CompanyResponse> UpdateCompany(CompanyDto company);
    }
}
