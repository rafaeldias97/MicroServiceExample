using System.Threading;
using System.Threading.Tasks;
using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.Repositories;
using MediatR;

namespace AccountService.Domain.Handlers
{
    public class TransferAccountHandler : IRequestHandler<TransferAccountRequest, TransferAccountResponse>
    {
        private readonly IAccountRepository accountRepository;
        public TransferAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public Task<TransferAccountResponse> Handle(
            TransferAccountRequest request,
            CancellationToken cancellationToken
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
            accountRepository.Commit();
            // Aciona evento para atualizar base nosql
            var result = new TransferAccountResponse(accountTo, accountFrom);
            return Task.FromResult(result);
        }
    }
}
