using System.Threading.Tasks;
using Backend.Core.Models.User;
using Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public AppDbContext appDbContext
        {
            get => _context as AppDbContext;
        }

        public UserRepository(AppDbContext context)
            : base(context) { }

        public async Task<int> GetCompanyIdByUserId(int userId)
        {
            var auth = await _context
                .Auth.Include(a => a.User)
                .FirstOrDefaultAsync(a => a.Id == userId);

            if (auth == null || auth.User == null)
            {
                return 0;
            }

            return auth.User.CompanyId ?? 0;
        }
    }
}
