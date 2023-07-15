namespace BookStore.Application.DTOs.Response
{
    public class CategoryResponse
    {
        public Guid CategoryGuid { get; set; }
        public string CategoryName { get; set; } = default!;
    }
}
