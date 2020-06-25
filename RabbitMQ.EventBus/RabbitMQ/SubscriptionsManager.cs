using RabbitMQ.EventBus.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;


namespace RabbitMQ.EventBus.RabbitMQ
{
    public class SubscriptionsManager
    {
        private readonly IDictionary<string, List<Subscription>> _handlers = new Dictionary<string, List<Subscription>>();

        /// <summary>
        /// Adição de eventos 
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        public void AddSubscription<TEvent, TEventHandler>()
            where TEvent : Event
            where TEventHandler : IEventHandler<TEvent> =>
            AddSubscription(typeof(TEventHandler), typeof(TEvent).Name, typeof(TEvent));

        /// <summary>
        /// Adição de eventos RPC
        /// </summary>
        /// <typeparam name="TEvent"></typeparam>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        public void AddSubscription<TEvent, TEventHandler, TResult>()
            where TEvent : Event
            where TEventHandler : IRPCEventHandler<TEvent, TResult> =>
            AddSubscription(typeof(TEventHandler), typeof(TEvent).Name, typeof(TEvent));

        private void AddSubscription(Type handlerType, string eventName, Type eventType)
        {
            if (!HasSubscriptionsForEvent(eventName))
            {
                _handlers.Add(eventName, new List<Subscription>());
            }

            if (_handlers[eventName].Any(s => s.HandlerType == handlerType))
            {
                throw new ArgumentException($"Handler Type {handlerType.Name} already registered for '{eventName}'", nameof(handlerType));
            }

            _handlers[eventName].Add(Subscription.New(handlerType, eventType));
        }

        public bool HasSubscriptionsForEvent(string eventName) => _handlers.ContainsKey(eventName);

        public IEnumerable<Subscription> GetHandlersForEvent(string eventName) => _handlers[eventName];
    }

}
