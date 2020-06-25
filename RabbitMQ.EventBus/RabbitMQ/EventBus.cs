using Autofac;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.EventBus.Interfaces;
using System;
using System.Collections.Concurrent;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RabbitMQ.EventBus.RabbitMQ
{
    public class EventBus : IEventBus, IDisposable
    {
        private readonly ILifetimeScope _autofac;
        private readonly IConnection _connection;
        private readonly string ExchangeName = "";
        private readonly SubscriptionsManager _subscriptionManager;
        private IModel _consumerChannel;
        private readonly EventBusSettings settings;

        public EventBus(ILifetimeScope autofac)
        {
            _autofac = autofac;

            if (!autofac.IsRegistered<EventBusSettings>())
                throw new ArgumentNullException("Registre as configurações do EventBus");
            else
                settings = autofac.Resolve<EventBusSettings>();

            var factory = new ConnectionFactory()
            {
                HostName = settings.HostName,
                VirtualHost = settings.VirtualHost,
                UserName = settings.UserName,
                Password = settings.Password,
                Port = settings.Port
            };

            _connection = factory.CreateConnection();

            _subscriptionManager = new SubscriptionsManager();

        }

        public void Publish(Event @event)
        {
            using (var channel = _connection.CreateModel())
            {
                var eventName = @event.GetType().Name;

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);

                channel.QueueDeclare(queue: eventName,
                                    durable: true,
                                    autoDelete: false,
                                    exclusive: false);

                channel.BasicPublish(exchange: ExchangeName,
                    routingKey: eventName,
                    basicProperties: null,
                    body: body);

            }
        }

        public T Publish<T>(Event @event)
        {
            using (var channel = _connection.CreateModel())
            {
                bool respondido = false;

                BlockingCollection<string> respQueue = new BlockingCollection<string>();
                IBasicProperties props = channel.CreateBasicProperties();
                EventingBasicConsumer consumer = new EventingBasicConsumer(channel);
                string replyQueueName = channel.QueueDeclare().QueueName;

                var correlationId = Guid.NewGuid().ToString();

                props.CorrelationId = correlationId;
                props.ReplyTo = replyQueueName;

                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body;
                    var response = Encoding.UTF8.GetString(body);
                    if (ea.BasicProperties.CorrelationId == correlationId)
                    {
                        respQueue.Add(response);
                    }
                };

                var messageBytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                channel.BasicPublish(
                    exchange: ExchangeName,
                    routingKey: @event.GetType().Name,
                    basicProperties: props,
                    body: messageBytes);

                channel.BasicConsume(
                    consumer: consumer,
                    queue: replyQueueName,
                    autoAck: true);


                Task.Run(() =>
                {
                    Thread.Sleep(settings.TimeoutRPC);

                    if (!respondido)
                        respQueue.Add("TIMEOUT");
                });

                var result = respQueue.Take();

                if (!string.IsNullOrEmpty(result) && result == "TIMEOUT")
                    throw new Exception("TIMEOUT");

                if (string.IsNullOrEmpty(result))
                    return default(T);

                return JsonConvert.DeserializeObject<T>(result);
            }
        }


        public void Subscribe<TEvent, TEventHandler>() where TEvent : Event where TEventHandler : IEventHandler<TEvent>
        {
            _consumerChannel = CreateConsumerChannel(typeof(TEvent).Name);
            _subscriptionManager.AddSubscription<TEvent, TEventHandler>();
        }

        private IModel CreateConsumerChannel(string eventName)
        {
            var channel = _connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            channel.QueueDeclare(
                queue: eventName,
                durable: true,
                autoDelete: false,
                exclusive: false);

            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                await HandleEvent(eventName, message);
            };


            channel.BasicConsume(
                queue: eventName,
                autoAck: true,
                consumer: consumer);

            channel.CallbackException += (sender, ea) =>
            {
                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel(eventName);
            };

            return channel;
        }

        private IModel CreateConsumerChannelRPC<TResult>(string eventName)
        {
            var channel = _connection.CreateModel();
            var consumer = new EventingBasicConsumer(channel);

            channel.QueueDeclare(
              queue: eventName,
              durable: true,
              exclusive: false,
              autoDelete: false,
              arguments: null);

            channel.BasicQos(0, 1, false);

            channel.BasicConsume(
                queue: eventName,
              autoAck: false,
              consumer: consumer);

            consumer.Received += async (model, ea) =>
            {
                var message = Encoding.UTF8.GetString(ea.Body);
                await HandleEvent<TResult>(eventName, message, channel, ea);
            };

            return channel;
        }

        private async Task HandleEvent<TResult>(string eventName, string message, IModel channel, BasicDeliverEventArgs ea)
        {
            if (!_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            using (var scope = _autofac.BeginLifetimeScope("RabbitMQEventBus"))
            {
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);
                var props = ea.BasicProperties;

                foreach (var subscription in subscriptions)
                {
                    string response = null;

                    var replyProps = channel.CreateBasicProperties();
                    replyProps.CorrelationId = props.CorrelationId;

                    try
                    {
                        var result = await subscription.Handle<TResult>(message, scope);
                        response = JsonConvert.SerializeObject(result);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(" [.] " + e.Message);
                        response = "";
                    }
                    finally
                    {
                        var responseBytes = Encoding.UTF8.GetBytes(response);

                        channel.BasicPublish(
                          exchange: "",
                          routingKey: props.ReplyTo,
                          basicProperties: replyProps,
                          body: responseBytes);

                        channel.BasicAck(
                          deliveryTag: ea.DeliveryTag,
                          multiple: false);
                    }
                }
            }
        }

        private async Task HandleEvent(string eventName, string message)
        {
            if (!_subscriptionManager.HasSubscriptionsForEvent(eventName))
            {
                return;
            }

            using (var scope = _autofac.BeginLifetimeScope("RabbitMQEventBus"))
            {
                var subscriptions = _subscriptionManager.GetHandlersForEvent(eventName);

                foreach (var subscription in subscriptions)
                {
                    await subscription.Handle(message, scope);
                }
            }
        }

        public void Dispose()
        {

            _connection.Close();
        }

        public void Subscribe<TEvent, TEventHandler, TResult>()
            where TEvent : Event
            where TEventHandler : IRPCEventHandler<TEvent, TResult>
        {
            _consumerChannel = CreateConsumerChannelRPC<TResult>(typeof(TEvent).Name);
            _subscriptionManager.AddSubscription<TEvent, TEventHandler, TResult>();
        }
    }

}
