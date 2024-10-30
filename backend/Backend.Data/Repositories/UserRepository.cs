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

        public async Task<UserResponse> Signup(SignupRequest signupRequestDto)
        {
            var user = new User
            {
                Username = signupRequestDto.Username,
                Email = signupRequestDto.Email,
                Password = signupRequestDto.Password,
            };

            await appDbContext.Users.AddAsync(user);
            await appDbContext.SaveChangesAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Message = "You have successfully registered.",
            };
        }

        public async Task<UserResponse> Signin(SigninRequest signinRequestDto)
        {
            var user = await appDbContext.Users.SingleOrDefaultAsync(u =>
                u.Email == signinRequestDto.Email
            );

            if (user == null)
            {
                return null;
            }

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = "Generated JWT token here",
            };
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<AllUsers>> GetUsers()
        {
            var users = await appDbContext
                .Users.Select(user => new AllUsers
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                })
                .ToListAsync();
            foreach (var user in users)
            {
                user.Username = user.Username;
                user.Email = user.Email;
                Console.WriteLine(user.Username);
            }
            return users;
        }

        public async Task<TheUser> GetUserById(int id)
        {
            var user = await appDbContext
                .Users.Select(user => new TheUser
                {
                    Id = user.Id,
                    Username = user.Username,
                    Email = user.Email,
                })
                .FirstOrDefaultAsync(user => user.Id == id);
            return user;
        }
    }
}
