using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Backend.Core.Models.Auth;
using Backend.Core.Models.Company;

namespace Backend.Core.Models.User
{
    public class User
    {
        public int Id { get; set; }

        public int AuthId { get; set; }
        public Backend.Core.Models.Auth.Auth Auth { get; set; }

        public int? CompanyId { get; set; }
        public Backend.Core.Models.Company.Company Company { get; set; }
    }
}
