
using Autofac;
using System;
using RabbitMQ.EventBus.Interfaces;

namespace RabbitMQ.EventBus.RabbitMQ
{
    public class EventBusService
    {
        private static IContainer _container;
        private static ContainerBuilder _builder;

        public EventBusService()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<EventBus>().As<IEventBus>();

        }

        /// <summary>
        /// Retorna uma instância do EventBus
        /// </summary>
        public IEventBus EventBus
        {
            get
            {
                return Container.Resolve<IEventBus>();
            }
        }

        /// <summary>
        /// Registra as configurações do EventBus
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public EventBusService Configure(Action<EventBusSettings> action)
        {
            var settings = new EventBusSettings();
            action(settings);
            _builder.RegisterInstance(settings);
            return this;
        }

        public EventBusService RegisterServices(Action<ContainerBuilder> action)
        {
            action(_builder);
            return this;
        }

        /// <summary>
        /// Registra os Handlers de inscrição no EventBus
        /// </summary>
        /// <typeparam name="TEventHandler"></typeparam>
        /// <returns></returns>
        public EventBusService RegisterHandle<TEventHandler>()
        {
            _builder.RegisterType<TEventHandler>();
            return this;
        }

        private static IContainer Container
        {
            get
            {
                if (_container == null)
                    _container = _builder.Build();

                return _container;
            }
        }

    }
}
