namespace Requests.BLL.DTOs.Requests
{
    public class UpdateRequestDto
    {
        public string RequestId { get; set; } = default!;
        public bool IsApproved { get; set; } = default!;
    }
}
