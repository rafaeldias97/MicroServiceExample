using System.Threading;
using System.Threading.Tasks;
using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using AccountService.Domain.Entities;
using AccountService.Domain.Repositories;
using MediatR;

namespace AccountService.Domain.Handlers
{
    public class CreditAccountHandler : IRequestHandler<CreditAccountRequest, CreditAccountResponse>
    {
        private readonly IAccountRepository accountRepository;
        public CreditAccountHandler(IAccountRepository accountRepository)
        {
            this.accountRepository = accountRepository;
        }
        public Task<CreditAccountResponse> Handle(
            CreditAccountRequest request,
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
            var result = new CreditAccountResponse();
            return Task.FromResult(result);
        }
    }
}
