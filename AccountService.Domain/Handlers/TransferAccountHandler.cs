using System.Threading;
using System.Threading.Tasks;
using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.EventBus;
using AccountService.Domain.Interfaces.Handlers;
using AccountService.Domain.Repositories;
using MediatR;

namespace AccountService.Domain.Handlers
{
    public class TransferAccountHandler : ITransferAccountHandler
    {
        private readonly IAccountRepository accountRepository;
        private readonly IExtractEventBus @event;
        public TransferAccountHandler(IAccountRepository accountRepository, IExtractEventBus @event)
        {
            this.accountRepository = accountRepository;
            this.@event = @event;
        }
        public Task<TransferAccountResponse> Handle(
            TransferAccountRequest request
            )
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
                // Consistência eventual na base nosql
                @event.Publisher(request);
            }
            var result = new TransferAccountResponse(accountTo, accountFrom);
            return Task.FromResult(result);
        }
    }
}
