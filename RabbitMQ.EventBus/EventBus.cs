using Newtonsoft.Json;
using RabbitMQ.Interfaces;
using System;

namespace RabbitMQ.EventBus
{
    public class EventBus<T> where T : class
    {
        private readonly IRabbitMQ rb;
        public EventBus(IRabbitMQ rb)
        {
            this.rb = rb;
        }

        public T Publisher(T entity)
        {
            rb.Publisher(typeof(T).Name, JsonConvert.SerializeObject(entity));
            return entity;
        }

        public void Subscribe(Action<string> cb)
        {
            // Exemplo Consumer
            rb.On(typeof(T).Name, (x) =>
            {
                cb.Invoke(x);
            });

            Console.ReadKey();
        }
    }
}
