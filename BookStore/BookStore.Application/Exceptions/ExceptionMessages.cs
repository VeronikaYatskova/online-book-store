namespace BookStore.Application.Exceptions
{
    public class ExceptionMessages
    {
        public const string BookNotFound = "Book is not found.";
        public const string BooksNotFoundMessage = "Books are not found.";
        public const string BookListIsEmpty = "Booklist is empty.";
        public const string NoFavoriteBooks = "Favorite books list is empty.";
        public const string CategoriesListIsEmpty = "No categories.";
        public const string AuthorNotFound = "Author is not found.";
        public const string PublishersListIsEmpty = "No publishers.";
        public const string BookAlreadyExists = "Book with the same title already exists.";
        public const string BookAlreadyExistsInFavorites = "Book was added to favorites before.";
    }
}
