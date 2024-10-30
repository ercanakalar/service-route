using Backend.Core.Models.User;

namespace Backend.Core.Services
{
    public interface IUserService
    {
        Task<UserResponse> Signup(SignupRequest request);
        Task<UserResponse> Signin(SigninRequest request);
        Task<List<AllUsers>> GetUsers();
        Task<TheUser> GetUserById(int id);
        Task<UserResponse> UpdateUser(UpdateUserRequest request);
        Task<UserResponse> UpdateTheUser(UpdateUserRequest request, int id);
    }
}
