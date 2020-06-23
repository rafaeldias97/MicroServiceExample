using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Queries.Reponses;
using AccountService.Domain.Queries.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountService.Domain.Repositories
{
    public interface IExtractRepository
    {
        Task<IEnumerable<ExtractAccountResponse>> Get(ExtractAccountRequest account);
        void SyncDb(TransferAccountRequest account);
    }
}
