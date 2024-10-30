using System.Threading.Tasks;
using Backend.Core.Models.Auth;
using Backend.Core.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories
{
    public class UserRepository : Repository<Auth>, IUserRepository
    {
        public AppDbContext appDbContext
        {
            get => _context as AppDbContext;
        }

        public UserRepository(AppDbContext context)
            : base(context) { }

        public async Task<AuthResponse> Signin(SigninRequest signinRequestDto)
        {
            var user = await appDbContext.Auth.SingleOrDefaultAsync(u =>
                u.Email == signinRequestDto.Email
            );

            if (user == null)
            {
                return null;
            }

            return new AuthResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = "Generated JWT token here",
            };
        }

        public async Task<Auth> GetByEmailAsync(string email)
        {
            return await _context.Auth.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<AllUsers>> GetUsers()
        {
            var users = await appDbContext
                .Auth.Select(user => new AllUsers
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles.ToList(),
                })
                .ToListAsync();
            foreach (var user in users)
            {
                user.Username = user.Username;
                user.Email = user.Email;
            }
            return users;
        }

        public async Task<AuthGeneralResponse> GetUserById(int id)
        {
            var user = await appDbContext
                .Auth.Select(user => new AuthGeneralResponse
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                    Roles = user.Roles.ToList(),
                })
                .FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }

        public async Task<Auth> GetByIdAsync(int id)
        {
            return await appDbContext.Auth.Include(a => a.User).FirstOrDefaultAsync(a => a.Id == id);
        }
    }
}
