namespace RabbitMQ.EventBus.Interfaces
{
    public interface IEventBus
    {
        /// <summary>
        /// Publicação de eventos
        /// </summary>
        /// <param name="event"></param>
        void Publish(Event @event);
        /// <summary>
        /// Publicação de eventos do tipo RPC 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="event"></param>
        /// <returns></returns>
        T Publish<T>(Event @event);

        /// <summary>
        /// Inscrição em eventos
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        void Subscribe<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent>;

        /// <summary>
        /// Inscrição em eventos RPC
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        void Subscribe<TEvent, TEventHandler, TResult>()
            where TEvent : Event
            where TEventHandler : IRPCEventHandler<TEvent, TResult>;
    }
}
