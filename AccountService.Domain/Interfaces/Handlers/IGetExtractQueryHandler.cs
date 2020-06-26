using AccountService.Domain.Queries.Reponses;
using AccountService.Domain.Queries.Requests;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccountService.Domain.Interfaces.Handlers
{
    public interface IGetExtractQueryHandler
    {
        Task<IEnumerable<ExtractAccountResponse>> Handle(ExtractAccountRequest request);
    }
}
