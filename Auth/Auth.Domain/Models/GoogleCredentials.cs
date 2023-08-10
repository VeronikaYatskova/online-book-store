namespace Auth.Domain.Models
{
    public class GoogleCredentials
    {
        public string ClientId { get; set; } = default!;
        public string ClientSecret { get; set; } = default!;
        public string RedirectUrl { get; set; } = default!;
    }
}
