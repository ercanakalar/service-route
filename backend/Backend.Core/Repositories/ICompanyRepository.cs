using System.Threading.Tasks;
using Backend.Core.Models.Company;

namespace Backend.Core.Repositories
{
    public interface ICompanyRepository : IRepository<CompanyDto>
    {
        Task<CompanyDto> GetByEmailAsync(string email);
        Task<CompanyDto> GetByNameAsync(string name);
        Task<CompanyDto> GetByIdAsync(int id);
    }
}
