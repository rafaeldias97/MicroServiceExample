using AccountService.Domain.Repositories;
using AccountService.Infra.Data.Context;
using AccountService.Infra.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AccountService.Domain.Handlers;
using RabbitMQ.EventBus.RabbitMQ;
using RabbitMQ.EventBus.Interfaces;
using AccountService.Domain.Commands.Requests;

namespace AccountService.Infra.IoC
{
    public static class NativeInjectorBootstraper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddDbContext<MSSQLContext>(x => x.UseSqlServer(@"Server=localhost;Database=AccountService;User Id=sa;Password=sa@12345;"));

            //services.AddSingleton(new Config("localhost", 5672, "guest", "guest").Start());
            var service = new EventBusService();
            service.RegisterHandle<TransferAccountCommandHandler>()
                .RegisterRepositoryServices<IAccountRepository, AccountRepository>()
                .Configure(settings =>
                {
                    settings.HostName = "localhost";
                    settings.VirtualHost = "/";
                    settings.UserName = "guest";
                    settings.Password = "guest";
                });
            IEventBus bus = service.EventBus;
            services.AddSingleton<IEventBus>(bus);
            bus.Subscribe<TransferAccountRequest, TransferAccountCommandHandler>();

            services.AddSingleton<IMongoContext>(new MongoContext { 
                CollectionName = "Account",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "AccountDb"
            });

        }
    }
}
