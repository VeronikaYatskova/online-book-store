using System.Linq.Expressions;
using Auth.Application.Abstractions.Interfaces.Repositories;
using Auth.Domain.Models;
using Auth.Infrastructure.Database.DataContext;

namespace Auth.Infrastructure.Database.Repositories
{
    public class AccountDataRepository : RepositoryBase<AccountData>, IAccountDataRepository
    {
        public AccountDataRepository(AppDbContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<AccountData>? FindAllAccounts() => FindAll();

        public AccountData? FindAccountBy(Expression<Func<AccountData, bool>> expression) =>
            FindByCondition(expression)?.FirstOrDefault();

        public AccountData? FindAccountById(Guid accountId) =>
            FindByCondition(u => u.Id == accountId)?.FirstOrDefault();
        
        public void AddAccount(AccountData account) => Create(account);

        public void UpdateAccount(AccountData account) => UpdateAccount(account);

        public async Task SaveAccountChangesAsync() => await SaveChangesAsync();
    }
}
