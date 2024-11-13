using Backend.Core.Models.Auth;

namespace Backend.Core.Services
{
    public interface IAuthService
    {
        Task<AuthResponse> CreateUser(SignupRequest request);
        Task<AuthResponse> Signin(SigninRequest request);
        Task<List<AllUsers>> GetUsers();
        Task<AuthGeneralResponse> GetUserById(int id);
        Task<AuthResponse> UpdateUser(UpdateUserRequest request);
        Task<AuthResponse> UpdateTheUser(UpdateUserRequest request, int id);
    }
}
