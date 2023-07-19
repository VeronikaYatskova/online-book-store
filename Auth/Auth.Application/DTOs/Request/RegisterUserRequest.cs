namespace Auth.Application.DTOs.Request
{
    public class RegisterUserRequest
    {
        public UserDataRequest UserDataRequest { get; set; } = default!;
        public RegisterAccountDataRequest RegisterAccountData { get; set; } = default!;
    }
}
