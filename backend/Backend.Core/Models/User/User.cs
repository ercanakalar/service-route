using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Models.Auth;
using Backend.Core.Models.Company;

namespace Backend.Core.Models.User
{
    public class UserDto
    {
        public int Id { get; set; }

        public int AuthId { get; set; }
        public Backend.Core.Models.Auth.AuthDto Auth { get; set; }

        public int PathId { get; set; }

        public int? CompanyId { get; set; }
        public Backend.Core.Models.Company.CompanyDto Company { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
