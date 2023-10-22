namespace OnlineBookStore.Messages.Models.Messages
{
    public class EmailConfirmationMessage
    {
        public string To { get; set; }
        public string ConfirmationLink { get; set; }    
    }
}