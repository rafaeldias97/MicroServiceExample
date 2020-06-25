namespace RabbitMQ.EventBus.RabbitMQ
{
    public class EventBusSettings
    {
        public string HostName { get; set; }
        public string VirtualHost { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int Port { get; set; } = 5672;
        public int TimeoutRPC { get; set; } = 30000;
    }

}
