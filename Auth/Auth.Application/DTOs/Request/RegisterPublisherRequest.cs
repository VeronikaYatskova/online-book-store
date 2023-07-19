namespace Auth.Application.DTOs.Request
{
    public class RegisterPublisherRequest
    {
        public PublisherDataRequest PublisherDataRequest { get; set; } = default!;
        public RegisterAccountDataRequest RegisterAccountData { get; set; } = default!;       
    }
}
