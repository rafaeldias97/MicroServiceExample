using AccountService.Domain.Commands.Requests;
using AccountService.Domain.EventBus;
using RabbitMQ.EventBus;
using RabbitMQ.Interfaces;

namespace AccountService.Infra.Data.EventBuss
{
    public class ExtractEventBus : EventBus<TransferAccountRequest>,
        IExtractEventBus
    {
        public ExtractEventBus(IRabbitMQ rb) : base(rb)
        {
        }
    }
}
