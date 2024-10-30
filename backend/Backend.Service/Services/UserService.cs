using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Error;
using Backend.Core.Models.User;
using Backend.Core.Services;
using Backend.Core.UnitOfWorks;
using Backend.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenGenerator _tokenService;
        private readonly PasswordManager _passwordManager;

        public UserService(
            IUnitOfWork unitOfWork,
            JwtTokenGenerator tokenService,
            PasswordManager passwordManager
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordManager = passwordManager;
        }

        public async Task<UserResponse> Signup(SignupRequest request)
        {
            var existingUser = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User with this email already exists",
                    StatusCode = 409,
                };
            }

            var hashedPassword = _passwordManager.HashPassword(request.Password);
            var hashedConfirmPassword = _passwordManager.HashPassword(request.Password);
            var user = new User
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                ConfirmPassword = hashedConfirmPassword,
                Roles = request.Roles ?? new List<string> { "User" },
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CommitAsync();

            var roles = user.Roles;
            var accessToken = _tokenService.GenerateToken(user, roles);
            var refreshToken = _tokenService.GenerateToken(user, roles);

            user.AccessToken = accessToken;
            user.RefreshToken = refreshToken;

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = accessToken,
                IsSuccess = true,
                StatusCode = 200,
            };
        }

        public async Task<UserResponse> Signin(SigninRequest request)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password",
                    StatusCode = 400,
                };
            }

            if (!_passwordManager.VerifyPassword(user.Password, request.Password))
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password",
                    StatusCode = 400,
                };
            }

            var roles = user.Roles;

            var accessToken = _tokenService.GenerateToken(user, roles);
            var refreshToken = _tokenService.GenerateToken(user, roles);

            var decoded = _tokenService.DecodeTokenAndCompareExpiry(
                user.AccessToken,
                user.RefreshToken
            );

            if (decoded.IsAccessTokenExpired)
            {
                user.RefreshToken = refreshToken;
            }

            user.AccessToken = _tokenService.GenerateExpiredToken(user, roles);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = accessToken,
                Message = "You have successfully signed in",
            };
        }

        public async Task<List<AllUsers>> GetUsers()
        {
            return await _unitOfWork.Users.GetUsers();
        }

        public async Task<TheUser> GetUserById(int id)
        {
            return await _unitOfWork.Users.GetUserById(id);
        }

        public async Task<UserResponse> UpdateUser(UpdateUserRequest request)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(request.Id);
            if (user == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = 404,
                };
            }

            user.Username = request.Username;
            user.Email = request.Email;
            user.Roles = request.Roles ?? user.Roles;
            user.Password = _passwordManager.HashPassword(request.Password);
            user.ConfirmPassword =
                request.Password ?? _passwordManager.HashPassword(request.Password);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = user.AccessToken,
                Message = "User updated successfully",
                IsSuccess = true,
                StatusCode = 200,
            };
        }

        public async Task<UserResponse> UpdateTheUser(UpdateUserRequest request, int id)
        {
            var user = await _unitOfWork.Users.GetByIdAsync(id);
            if (user == null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = 404,
                };
            }

            var isThereUserName = await _unitOfWork.Users.GetByUsernameAsync(request.Username);
            if (isThereUserName != null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Username already exists",
                    StatusCode = 409,
                };
            }
            user.Username = request.Username;

            var isThereEmail = await _unitOfWork.Users.GetByEmailAsync(request.Email);
            if (isThereEmail != null)
            {
                return new UserResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Email already exists",
                    StatusCode = 409,
                };
            }
            user.Email = request.Email;

            user.Password = _passwordManager.HashPassword(request.Password);
            user.ConfirmPassword =
                request.Password ?? _passwordManager.HashPassword(request.Password);

            _unitOfWork.Users.Update(user);
            await _unitOfWork.CommitAsync();

            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = user.AccessToken,
                Message = "User updated successfully",
                IsSuccess = true,
                StatusCode = 200,
            };
        }
    }
}
