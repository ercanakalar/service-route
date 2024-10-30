using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Auth;
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

        public async Task<AuthResponse> CreateUser(SignupRequest request)
        {
            var existingUser = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
            if (existingUser != null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Auth with this email already exists",
                    StatusCode = 409,
                };
            }

            var hashedPassword = _passwordManager.HashPassword(request.Password);
            var hashedConfirmPassword = _passwordManager.HashPassword(request.Password);
            var auth = new Auth
            {
                Username = request.Username,
                Email = request.Email,
                Password = hashedPassword,
                ConfirmPassword = hashedConfirmPassword,
                Roles = request.Roles ?? new List<string> { "User" },
            };

            var user = new User
            {
                Auth = auth
            };

            auth.User = user;

            await _unitOfWork.Auth.AddAsync(auth);
            await _unitOfWork.CommitAsync();

            var roles = auth.Roles;
            var accessToken = _tokenService.GenerateToken(auth, roles);
            var refreshToken = _tokenService.GenerateToken(auth, roles);

            auth.AccessToken = accessToken;
            auth.RefreshToken = refreshToken;

            _unitOfWork.Auth.Update(auth);
            await _unitOfWork.CommitAsync();

            return new AuthResponse
            {
                Id = auth.Id,
                Username = auth.Username,
                Email = auth.Email,
                Token = accessToken,
                IsSuccess = true,
                StatusCode = 200,
            };
        }

        public async Task<AuthResponse> Signin(SigninRequest request)
        {
            var user = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Invalid email or password",
                    StatusCode = 400,
                };
            }

            if (!_passwordManager.VerifyPassword(user.Password, request.Password))
            {
                return new AuthResponse
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

            _unitOfWork.Auth.Update(user);
            await _unitOfWork.CommitAsync();

            return new AuthResponse
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
            return await _unitOfWork.Auth.GetUsers();
        }

        public async Task<AuthGeneralResponse> GetUserById(int id)
        {
            return await _unitOfWork.Auth.GetUserById(id);
        }

        public async Task<AuthResponse> UpdateUser(UpdateUserRequest request)
        {
            var existUser = await _unitOfWork.Auth.GetByIdAsync(request.Id);
            if (existUser == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = 404,
                };
            }

            if (existUser.User == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Associated user not found",
                    StatusCode = 404,
                };
            }

            if (request.CompanyId != null)
            {
                existUser.User.CompanyId = request.CompanyId;
            }

            existUser.Username = request.Username;
            existUser.Email = request.Email;
            existUser.Roles = request.Roles ?? existUser.Roles;
            existUser.Password = _passwordManager.HashPassword(request.Password);
            existUser.ConfirmPassword =
                request.Password ?? _passwordManager.HashPassword(request.Password);

            _unitOfWork.Auth.Update(existUser);

            await _unitOfWork.CommitAsync();

            return new AuthResponse
            {
                Id = existUser.Id,
                Username = existUser.Username,
                Email = existUser.Email,
                Token = existUser.AccessToken,
                Message = "Auth updated successfully",
                IsSuccess = true,
                StatusCode = 200,
            };
        }

        public async Task<AuthResponse> UpdateTheUser(UpdateUserRequest request, int id)
        {
            var user = await _unitOfWork.Auth.GetByIdAsync(id);
            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "User not found",
                    StatusCode = 404,
                };
            }

            var isThereUserName = await _unitOfWork.Auth.GetByUsernameAsync(request.Username);
            if (isThereUserName != null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Username already exists",
                    StatusCode = 409,
                };
            }
            user.Username = request.Username;

            var isThereEmail = await _unitOfWork.Auth.GetByEmailAsync(request.Email);
            if (isThereEmail != null)
            {
                return new AuthResponse
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

            _unitOfWork.Auth.Update(user);
            await _unitOfWork.CommitAsync();

            return new AuthResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = user.AccessToken,
                Message = "Auth updated successfully",
                IsSuccess = true,
                StatusCode = 200,
            };
        }
    }
}
