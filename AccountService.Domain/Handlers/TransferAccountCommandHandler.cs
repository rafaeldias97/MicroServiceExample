using System.Threading.Tasks;
using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.Repositories;
using RabbitMQ.EventBus.Interfaces;

namespace AccountService.Domain.Handlers
{
    public class TransferAccountCommandHandler : IEventHandler<TransferAccountRequest>
    {
        private readonly IAccountRepository accountRepository;
        private readonly IEventBus @event;
        public TransferAccountCommandHandler(IAccountRepository accountRepository, IEventBus @event)
        {
            this.accountRepository = accountRepository;
            this.@event = @event;
        }
        public Task Handle(TransferAccountRequest request)
        {
            var accountTo = new Account(request.To.Number, request.To.DueDate, request.To.SecurityCode);
            var accountFrom = new Account(request.From.Number, request.From.DueDate, request.From.SecurityCode);
            /**
                Implementação de verificação de saldo da conta origem
            **/
            // debita conta origem
            accountRepository.Debit(accountTo);
            // credita conta destino
            accountRepository.Credit(accountFrom);
            // Realiza Commit
            if (accountRepository.Commit())
            {
                var extractTo = new GenerateExtractRequest(accountTo.CreationDate, $"Transferência realizada para  conta {accountFrom.Number}", accountTo.Value, accountTo.Number, 50);
                var extractFrom = new GenerateExtractRequest(accountFrom.CreationDate, $"Transferência recebida da conta {accountTo.Number}", accountFrom.Value, accountFrom.Number, 50);
                // Consistência eventual na base nosql
                @event.Publish(extractTo);
                @event.Publish(extractFrom);
            }
            var result = new TransferAccountResponse(accountTo, accountFrom);
            return Task.FromResult(result);
        }
    }
}
