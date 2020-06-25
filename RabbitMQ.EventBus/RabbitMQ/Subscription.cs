using Autofac;
using Newtonsoft.Json;
using RabbitMQ.EventBus.Interfaces;
using System;
using System.Threading.Tasks;

namespace RabbitMQ.EventBus.RabbitMQ
{
    public class Subscription
    {
        public Type HandlerType { get; }
        public Type EventType { get; }

        private Subscription(Type handlerType, Type eventType)
        {
            HandlerType = handlerType;
            EventType = eventType;
        }

        public async Task Handle(string message, ILifetimeScope scope)
        {
            var eventData = JsonConvert.DeserializeObject(message, EventType);
            var handler = scope.ResolveOptional(HandlerType);
            var concreteType = typeof(IEventHandler<>).MakeGenericType(EventType);
            await (Task)concreteType.GetMethod("Handle").Invoke(handler, new[] { eventData });
        }

        public async Task<TResult> Handle<TResult>(string message, ILifetimeScope scope)
        {
            var eventData = JsonConvert.DeserializeObject(message, EventType);
            var handler = scope.ResolveOptional(HandlerType);
            var concreteType = typeof(IRPCEventHandler<,>).MakeGenericType(EventType, typeof(TResult));
            return await (Task<TResult>)concreteType.GetMethod("Handle").Invoke(handler, new[] { eventData });
        }

        public static Subscription New(Type handlerType, Type eventType) => new Subscription(handlerType, eventType);
    }
}
