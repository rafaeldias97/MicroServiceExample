using System;

namespace RabbitMQ.EventBus.Interfaces
{
    public abstract class Event
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        protected Event()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }
    }
}
