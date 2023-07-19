using System.Linq.Expressions;
using Auth.Domain.Models;

namespace Auth.Application.Abstractions.Interfaces.Repositories
{
    public interface IAccountDataRepository
    {
        IEnumerable<AccountData>? FindAllAccounts();    
        AccountData? FindAccountById(Guid accountId);
        AccountData? FindAccountBy(Expression<Func<AccountData, bool>> expression);
        void AddAccount(AccountData account);
        void UpdateAccount(AccountData account);
        Task SaveChangesAsync();
    }
}
