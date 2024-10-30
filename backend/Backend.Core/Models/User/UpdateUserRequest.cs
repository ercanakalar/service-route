namespace Backend.Core.Models.User
{
    public class UpdateUserRequest
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public List<string>? Roles { get; set; } = new List<string> { };
    }
}
