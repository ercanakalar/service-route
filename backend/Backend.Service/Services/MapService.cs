using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Core.Models.Error;
using Backend.Core.Models.Map;
using Backend.Core.Services;
using Backend.Core.UnitOfWorks;
using Backend.Service.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service.Services
{
    public class MapService : IMapService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly JwtTokenGenerator _tokenService;
        private readonly PasswordManager _passwordManager;

        public MapService(
            IUnitOfWork unitOfWork,
            JwtTokenGenerator tokenService,
            PasswordManager passwordManager
        )
        {
            _unitOfWork = unitOfWork;
            _tokenService = tokenService;
            _passwordManager = passwordManager;
        }

        public async Task<MapResponse> InsertWaypoints(MapRequest request, int userId)
        {
            var company = await _unitOfWork.Companies.GetByIdAsync(request.CompanyId);
            if (company == null)
            {
                return new MapResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company not found",
                    StatusCode = 404,
                };
            }

            var companyId = await _unitOfWork.Auth.GetCompanyIdByUserId(userId);

            var map = new WaypointsDto
            {
                Address = request.Address,
                Latitude = request.Latitude,
                Longitude = request.Longitude,
                PathId = request.PathId,
                Order = request.Order,
                CompanyId = request.CompanyId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _unitOfWork.Waypoints.AddAsync(map);
            await _unitOfWork.CommitAsync();

            return new MapResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Waypoints = new List<WaypointsDto> { map },
            };
        }

        public async Task<MapResponse> GetWaypointsAsAdmin()
        {
            var waypoints = await _unitOfWork.Waypoints.GetAllAsync();
            if (waypoints == null)
            {
                return new MapResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Waypoints not found",
                    StatusCode = 404,
                };
            }

            return new MapResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Waypoints = waypoints.ToList(),
            };
        }

        public async Task<MapResponse> GetWaypointsAsUser(int userId)
        {
            var companyId = await _unitOfWork.Auth.GetCompanyIdByUserId(userId);
            Console.WriteLine(companyId);
            if (companyId == 0)
            {
                return new MapResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Company not found",
                    StatusCode = 404,
                };
            }

            var waypoints = await _unitOfWork.Waypoints.GetWaypointsByCompanyId(companyId);
            if (waypoints == null)
            {
                return new MapResponse
                {
                    IsSuccess = false,
                    ErrorMessage = "Waypoints not found",
                    StatusCode = 404,
                };
            }

            return new MapResponse
            {
                IsSuccess = true,
                StatusCode = 200,
                Waypoints = waypoints,
            };
        }
    }
}
