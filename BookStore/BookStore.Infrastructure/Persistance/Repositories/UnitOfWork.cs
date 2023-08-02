using BookStore.Application.Abstractions.Contracts.Interfaces;
using BookStore.Domain.Entities;
using BookStore.Infrastructure.Persistance.DataContext;

namespace BookStore.Infrastructure.Persistance.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _dbContext;
        private IRepositoryBase<BookEntity> _booksRepository;
        private IRepositoryBase<CategoryEntity> _categoryRepository;
        private IRepositoryBase<BookAuthorEntity> _authorsBooksRepository;
        private IRepositoryBase<UserBookEntity> _userBooksRepository;
        private IRepositoryBase<User> _usersRepository;

        public UnitOfWork(
            AppDbContext dbContext, 
            IRepositoryBase<BookEntity> booksRepository, 
            IRepositoryBase<CategoryEntity> categoryRepository, 
            IRepositoryBase<BookAuthorEntity> authorsBooksRepository, 
            IRepositoryBase<UserBookEntity> userBooksRepository,
            IRepositoryBase<User> usersRepository)
        {
            _dbContext = dbContext;
            _booksRepository = booksRepository;
            _categoryRepository = categoryRepository;
            _authorsBooksRepository = authorsBooksRepository;
            _userBooksRepository = userBooksRepository;
            _usersRepository = usersRepository;
        }

        public IRepositoryBase<BookEntity> BooksRepository
        {
            get 
            {
                _booksRepository ??= new RepositoryBase<BookEntity>(_dbContext);

                return _booksRepository; 
            }
            
        }

        public IRepositoryBase<CategoryEntity> CategoryRepository
        {
            get 
            {
                _categoryRepository ??= new RepositoryBase<CategoryEntity>(_dbContext);

                return _categoryRepository; 
            }
        }

        public IRepositoryBase<BookAuthorEntity> AuthorsBooksRepository
        {
            get 
            {
                _authorsBooksRepository ??= new RepositoryBase<BookAuthorEntity>(_dbContext);

                return _authorsBooksRepository; 
            }
        }

        public IRepositoryBase<UserBookEntity> UserBooksRepository
        {
            get 
            {
                _userBooksRepository ??= new RepositoryBase<UserBookEntity>(_dbContext);

                return _userBooksRepository; 
            }
        }

        public IRepositoryBase<User> UsersRepository
        {
            get
            {
                _usersRepository ??= new RepositoryBase<User>(_dbContext);

                return _usersRepository;
            }
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
