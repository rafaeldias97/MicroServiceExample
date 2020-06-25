using System.Threading.Tasks;

namespace RabbitMQ.EventBus.Interfaces
{
    public interface IEventHandler<TEvent> where TEvent : Event
    {
        Task Handle(TEvent @event);
    }

    public interface IRPCEventHandler<TEvent, TResult>
        where TEvent : Event
    {
        Task<TResult> Handle(TEvent @event);
    }
}
