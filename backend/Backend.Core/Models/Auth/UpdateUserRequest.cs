namespace Backend.Core.Models.Auth
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int CompanyId { get; set; }
        public string Password { get; set; } = string.Empty;

        public List<string>? Roles { get; set; } = new List<string> { };
    }
}
