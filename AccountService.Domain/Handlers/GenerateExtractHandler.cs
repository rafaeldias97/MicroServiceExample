using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AccountService.Domain.Queries.Reponses;
using AccountService.Domain.Queries.Requests;
using AccountService.Domain.Repositories;
using MediatR;

namespace AccountService.Domain.Handlers
{
    public class GenerateExtractHandler : IRequestHandler<ExtractAccountRequest, IEnumerable<ExtractAccountResponse>>
    {
        private readonly IExtractRepository extractRepository;
        public GenerateExtractHandler(IExtractRepository extractRepository)
        {
            this.extractRepository = extractRepository;
        }
        public Task<IEnumerable<ExtractAccountResponse>> Handle(
            ExtractAccountRequest request,
            CancellationToken cancellationToken
            )
        {
            var result = extractRepository.Get(request);
            return result;
        }
    }
}
