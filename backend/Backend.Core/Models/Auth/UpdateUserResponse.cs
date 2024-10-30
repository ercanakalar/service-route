namespace Backend.Core.Models.Auth
{
    public class UpdateAuthResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string> { };

        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; } = 200;
    }
}
