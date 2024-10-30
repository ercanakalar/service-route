using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Core.Models.User;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Service.Helpers
{
    public class PasswordManager
    {
        public string HashPassword(string password)
        {
            var salt = GenerateSalt();
            var hashedPassword = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return $"{Convert.ToBase64String(salt)}:{hashedPassword}";
        }

        public bool IsMatchPasswords(string password, string hashedPassword)
        {
            var parts = hashedPassword.Split(':');
            var salt = Convert.FromBase64String(parts[0]);
            var hash = parts[1];

            var hashedAttempt = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: password,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return hash == hashedAttempt;
        }

        private byte[] GenerateSalt()
        {
            var salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        public string GenerateJwtToken(User user, string key)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username),
            };

            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
                    SecurityAlgorithms.HmacSha256Signature
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public bool VerifyPassword(string hashedPassword, string plainTextPassword)
        {
            var parts = hashedPassword.Split(':');
            if (parts.Length != 2)
                throw new FormatException(
                    "Unexpected hash format. Expected format: `{salt}:{hashedPassword}`"
                );

            var salt = Convert.FromBase64String(parts[0]);
            var storedHash = parts[1];

            var hashedAttempt = Convert.ToBase64String(
                KeyDerivation.Pbkdf2(
                    password: plainTextPassword,
                    salt: salt,
                    prf: KeyDerivationPrf.HMACSHA256,
                    iterationCount: 10000,
                    numBytesRequested: 256 / 8
                )
            );

            return hashedAttempt == storedHash;
        }
    }
}
