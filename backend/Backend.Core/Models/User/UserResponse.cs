namespace Backend.Core.Models.User
{
    public class UserResponse
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Token { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = true;
        public string ErrorMessage { get; set; }
        public int StatusCode { get; set; } = 200;
    }
}
