using System.Threading.Tasks;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.Repositories;
using RabbitMQ.EventBus.Interfaces;

namespace AccountService.Domain.Handlers
{
    public class GenerateExtractCommandHandler : IEventHandler<GenerateExtractRequest>
    {
        private readonly IExtractRepository extractRepository;
        public GenerateExtractCommandHandler(IExtractRepository extractRepository)
        {
            this.extractRepository = extractRepository;
        }

        public Task Handle(GenerateExtractRequest request)
        {
            var extract = new Extract(request.DatTransaction, request.Historic, request.Value, request.NumberAccount, request.Balance);
            extractRepository.SyncDb(extract);
            return Task.FromResult(extract);
        }
    }
}
