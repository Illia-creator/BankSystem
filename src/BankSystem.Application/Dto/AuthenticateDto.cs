namespace BankSystem.Application.Dto
{
    public class AuthenticateDto
    {
        public Guid AccountNumber { get; set; }
        public string Pin { get; set; }
    }
}
