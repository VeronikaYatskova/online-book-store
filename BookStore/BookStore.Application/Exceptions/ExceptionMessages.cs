namespace BookStore.Application.Exceptions
{
    public class ExceptionMessages
    {
        public const string BookNotFoundMessage = "Book is not found.";
        public const string BooksNotFoundMessage = "Books are not found.";
        public const string BookListIsEmptyMessage = "Booklist is empty.";
        public const string NoFavoriteBooksMessage = "Favorite books list is empty.";
        public const string CategoriesListIsEmptyMessage = "No categories.";
        public const string AuthorNotFoundMessage = "Author is not found.";
        public const string PublishersListIsEmptyMessage = "No publishers.";
        public const string BookAlreadyExistsMessage = "Book with the same title already exists.";
        public const string BookAlreadyExistsInFavoritesMessage = "Book was added to favorites before.";
    }
}
