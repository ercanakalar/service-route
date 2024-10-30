namespace Backend.Core.Models.Jwt
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInDays { get; set; }
    }
}
