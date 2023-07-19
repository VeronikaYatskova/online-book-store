namespace Auth.Application.DTOs.Request
{
    public class RegisterAuthorRequest
    {
        public AuthorDataRequest AuthorDataRequest { get; set; } = default!;
        public RegisterAccountDataRequest RegisterAccountData { get; set; } = default!; 
    }
}
