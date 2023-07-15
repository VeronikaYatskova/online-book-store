using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext dbContext;
        private IBookRepository bookRepository;
        private IPublisherRepository publisherRepository;
        private ICategoryRepository categoryRepository;
        private IAuthorRepository authorRepository;
        private IAuthorsBooksRepository authorsBooksRepository;
        private IFavoriteBooksRepository favoriteBooksRepository;

        public UnitOfWork(
            AppDbContext dbContext, 
            IBookRepository bookRepository, 
            IPublisherRepository publisherRepository, 
            ICategoryRepository categoryRepository,
            IAuthorRepository authorRepository,
            IAuthorsBooksRepository authorsBooksRepository,
            IFavoriteBooksRepository favoriteBooksRepository)
        {
            this.dbContext = dbContext;
            this.bookRepository = bookRepository;
            this.publisherRepository = publisherRepository;
            this.categoryRepository = categoryRepository;
            this.authorRepository = authorRepository;
            this.authorsBooksRepository = authorsBooksRepository;
            this.favoriteBooksRepository = favoriteBooksRepository;
        }

        public IBookRepository BooksRepository
        {
            get 
            {
                bookRepository ??= new BookRepository(dbContext);

                return bookRepository;
            }
        }

        public IPublisherRepository PublishersRepository
        {
            get 
            {
                publisherRepository ??= new PublisherRepository(dbContext);

                return publisherRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get 
            {
                categoryRepository ??= new CategoryRepository(dbContext);

                return categoryRepository;
            }
        }

        public IAuthorRepository AuthorRepository
        {
            get
            {
                authorRepository ??= new AuthorRepository(dbContext);

                return authorRepository;
            }
        }

        public IAuthorsBooksRepository AuthorsBooksRepository
        {
            get 
            {
                authorsBooksRepository ??= new AuthorsBooksRepository(dbContext);

                return authorsBooksRepository;
            }
        }

        public IFavoriteBooksRepository FavoriteBooksRepository
        {
            get
            {
                favoriteBooksRepository ??= new FavoriteBooksRepository(dbContext);

                return favoriteBooksRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
