using System.Collections.Generic;
using Backend.Core.Models.Company;

namespace Backend.Core.Models.Company
{
    public class CompanyResponse
    {
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; } = 200;
        public CompanyDto? Company { get; set; }
    }
}
