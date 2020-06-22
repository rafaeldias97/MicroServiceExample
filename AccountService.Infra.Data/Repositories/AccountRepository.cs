using System.Threading.Tasks;
using AccountService.Domain.Entities;
using AccountService.Domain.Repositories;
using AccountService.Infra.Data.Context;

namespace AccountService.Infra.Data.Repositories
{
    public class AccountRepository : IAccountRepository
    {
        private readonly MSSQLContext ctx;
        public AccountRepository(MSSQLContext ctx)
        {
            this.ctx = ctx;
        }
        public void Commit()
        {
            ctx.SaveChanges();
        }

        public Task<Account> Credit(Account account)
        {
            var res = ctx.Account.Add(account);
            return Task.FromResult(res.Entity);
        }

        public Task<Account> Debit(Account account)
        {
            account.Number = account.Number * -1;
            var res = ctx.Account.Add(account);
            return Task.FromResult(res.Entity);
        }
    }
}
