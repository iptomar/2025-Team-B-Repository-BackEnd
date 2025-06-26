namespace WebApplication1.Models
{
    public class Register
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
        public string? Role { get; set; }
    }
}
