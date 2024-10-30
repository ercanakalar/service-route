using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Backend.Core.Models.Jwt;
using Backend.Core.Models.Auth;
using Backend.Core.Repositories;
using Backend.Core.Services;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service.Helpers
{
    public class JwtTokenGenerator
    {
        private readonly JwtOptions _jwtOptions;

        public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
        {
            _jwtOptions = jwtOptions?.Value ?? throw new ArgumentNullException(nameof(jwtOptions));

            _jwtOptions.Secret = !string.IsNullOrEmpty(_jwtOptions.Secret)
                ? _jwtOptions.Secret
                : "DefaultSuperSecretKeyForTesting123!";
            _jwtOptions.ExpiryInDays = _jwtOptions.ExpiryInDays > 0 ? _jwtOptions.ExpiryInDays : 1;
        }

        public string GenerateToken(Auth applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.Username),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeOffset
                    .UtcNow.AddDays(_jwtOptions.ExpiryInDays)
                    .ToOffset(TimeSpan.FromHours(3))
                    .DateTime,
                NotBefore = DateTimeOffset
                    .UtcNow.AddMinutes(-5)
                    .ToOffset(TimeSpan.FromHours(3))
                    .DateTime,
                IssuedAt = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(3)).DateTime,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public string GenerateExpiredToken(Auth applicationUser, IEnumerable<string> roles)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, applicationUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, applicationUser.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, applicationUser.Username),
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTimeOffset
                    .UtcNow.AddDays(_jwtOptions.ExpiryInDays)
                    .ToOffset(TimeSpan.FromHours(3))
                    .DateTime,
                NotBefore = DateTimeOffset
                    .UtcNow.AddMinutes(-5)
                    .ToOffset(TimeSpan.FromHours(3))
                    .DateTime,
                IssuedAt = DateTimeOffset.UtcNow.ToOffset(TimeSpan.FromHours(3)).DateTime,
                Issuer = _jwtOptions.Issuer,
                Audience = _jwtOptions.Audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature
                ),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ClaimsPrincipal DecodeToken(string accessToken, string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

            var accessTokenPrincipal = ValidateToken(accessToken);
            var refreshTokenPrincipal = ValidateToken(refreshToken);

            if (accessTokenPrincipal == null || refreshTokenPrincipal == null)
            {
                return null;
            }

            return accessTokenPrincipal;
        }

        public (
            DateTime? AccessTokenExpires,
            DateTime? RefreshTokenExpires,
            bool IsAccessTokenExpired
        ) DecodeTokenAndCompareExpiry(string accessToken, string refreshToken)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            var accessTokenPrincipal = ValidateToken(accessToken);
            var refreshTokenPrincipal = ValidateToken(refreshToken);

            if (accessTokenPrincipal == null || refreshTokenPrincipal == null)
            {
                return (null, null, true);
            }

            var accessTokenExpires = accessTokenPrincipal
                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
                ?.Value;

            var refreshTokenExpires = refreshTokenPrincipal
                .Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)
                ?.Value;

            var accessTokenExpiryDate =
                accessTokenExpires != null
                    ? UnixTimeStampToDateTime(long.Parse(accessTokenExpires))
                    : (DateTime?)null;
            var refreshTokenExpiryDate =
                refreshTokenExpires != null
                    ? UnixTimeStampToDateTime(long.Parse(refreshTokenExpires))
                    : (DateTime?)null;

            bool isAccessTokenExpired =
                accessTokenExpiryDate.HasValue && accessTokenExpiryDate.Value < DateTime.UtcNow;

            return (accessTokenExpiryDate, refreshTokenExpiryDate, isAccessTokenExpired);
        }

        private DateTime UnixTimeStampToDateTime(long unixTimeStamp)
        {
            return DateTimeOffset.FromUnixTimeSeconds(unixTimeStamp).UtcDateTime;
        }

        private ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtOptions.Secret);

            try
            {
                var claimsPrincipal = tokenHandler.ValidateToken(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidIssuer = _jwtOptions.Issuer,
                        ValidateAudience = true,
                        ValidAudience = _jwtOptions.Audience,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken
                );

                return claimsPrincipal;
            }
            catch
            {
                return null;
            }
        }
    }
}
