namespace BookStore.Domain.Exceptions
{
    public class FileDoesNotExistException : Exception
    {
        public FileDoesNotExistException() { }
        public FileDoesNotExistException(string message)
            : base(message) { }
        public FileDoesNotExistException(string message, Exception inner)
            : base(message, inner) { }
    }
}