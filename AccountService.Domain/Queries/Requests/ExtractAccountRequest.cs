using AccountService.Domain.Queries.Reponses;
using MediatR;
using System.Collections.Generic;

namespace AccountService.Domain.Queries.Requests
{
    public class ExtractAccountRequest : IRequest<IEnumerable<ExtractAccountResponse>>
    {
        public long NumberAccountFrom { get; set; }
    }
}
