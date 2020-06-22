using AccountService.Domain.Commands.Requests;

namespace AccountService.Domain.EventBus
{
    public interface IExtractEventBus : IBaseEventBus<TransferAccountRequest>
    {
    }
}
