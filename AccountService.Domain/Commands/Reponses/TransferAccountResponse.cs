using AccountService.Domain.Entities;

namespace AccountService.Domain.Commands.Reponses
{
    public class TransferAccountResponse
    {
        public TransferAccountResponse(Account accountTo, Account accountFrom)
        {
            AccountTo = accountTo;
            AccountFrom = accountFrom;
        }
        public Account AccountTo { get; set; }
        public Account AccountFrom { get; set; }

        public override string ToString()
        {
            return $"Transferencia da conta {AccountTo.Number} para {AccountFrom.Number} com o valor de {AccountTo.Value}R$ Realizado com sucesso";
        }
    }
}
