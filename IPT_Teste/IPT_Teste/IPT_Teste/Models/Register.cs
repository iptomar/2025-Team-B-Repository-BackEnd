namespace IPT_Teste.Models
{
    public class Register
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string[] Roles { get; set; }
    }
}
