namespace BookStore.Application.DTOs.Request
{
    public class SendBookInfoDto
    {
        public string BookName { get; set; } = default!;   
        public string[] BookAuthors { get; set; } = default!;
        public string PublishDate { get; set; } = default!;
        public string PublisherName { get; set; } = default!;        
    }
}