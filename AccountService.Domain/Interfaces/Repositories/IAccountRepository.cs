using AccountService.Domain.Entities;
using System.Threading.Tasks;

namespace AccountService.Domain.Repositories
{
    public interface IAccountRepository
    {
        Task<Account> Debit(Account account);
        Task<Account> Credit(Account account);
        bool Commit();
    }
}
