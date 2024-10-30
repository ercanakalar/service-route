using System.Threading.Tasks;
using Backend.Core.Models.Company;
using Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    public class CompanyRepository : Repository<Company>, ICompanyRepository
    {
        public AppDbContext appDbContext
        {
            get => _context as AppDbContext;
        }

        public CompanyRepository(AppDbContext context)
            : base(context) { }

        public async Task<Company> GetByEmailAsync(string email)
        {
            return await _context
                .Companies.AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
