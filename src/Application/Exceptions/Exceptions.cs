namespace Application.Exceptions
{
    public class Exceptions
    {
        public static NotFoundException BookNotFound = new("Book is not found.");
        public static NotFoundException BookListIsEmpty = new("Booklist is empty.");
        public static NotFoundException NoFavoriteBooks = new("Favorite books list is empty.");
        public static NotFoundException CategoriesListIsEmpty = new("No categories.");
        public static NotFoundException AuthorNotFound = new("Author is not found.");
        public static NotFoundException PublishersListIsEmpty = new("No publishers.");
        public static AlreadyExistsException BookAlreadyExists = new("Book with the same title already exists.");
        public static AlreadyExistsException BookAlreadyExistsInFavorites = new("Book was added to favorites before.");
    }
}
