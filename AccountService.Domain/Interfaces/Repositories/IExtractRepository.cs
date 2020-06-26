using AccountService.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountService.Domain.Repositories
{
    public interface IExtractRepository
    {
        Task<IEnumerable<Extract>> Get(long numberAccount);
        void SyncDb(Extract account);
    }
}
