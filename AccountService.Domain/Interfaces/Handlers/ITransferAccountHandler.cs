using AccountService.Domain.Commands.Reponses;
using AccountService.Domain.Commands.Requests;
using System.Threading.Tasks;

namespace AccountService.Domain.Interfaces.Handlers
{
    public interface ITransferAccountHandler
    {
        Task<TransferAccountResponse> Handle(TransferAccountRequest request);
    }
}
