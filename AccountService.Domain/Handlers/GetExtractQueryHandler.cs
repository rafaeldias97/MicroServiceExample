using AccountService.Domain.Interfaces.Handlers;
using AccountService.Domain.Queries.Reponses;
using AccountService.Domain.Queries.Requests;
using AccountService.Domain.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService.Domain.Handlers
{
    public class GetExtractQueryHandler : IGetExtractQueryHandler
    {
        private readonly IExtractRepository extractRepository;
        public GetExtractQueryHandler(IExtractRepository extractRepository)
        {
            this.extractRepository = extractRepository;
        }

        public async Task<IEnumerable<ExtractAccountResponse>> Handle(ExtractAccountRequest request)
        {
            var res = await extractRepository.Get(request.NumberAccount);
            var extract = res.Select(s => new ExtractAccountResponse(s.DatTransaction, s.Historic, s.Value, s.NumberAccount, s.Balance));
            return extract;
        }
    }
}
