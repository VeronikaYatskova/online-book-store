namespace Requests.BLL.DTOs.General
{
    public class BlobManipulation
    {
        public string? Status { get; set; }
        public bool Error { get; set; }
        public Blob Blob { get; set; }

        public BlobManipulation()
        {
            Blob = new Blob();
        }
    }
}