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
using Autofac;
using AccountService.Domain.Interfaces.Handlers;

namespace AccountService.Infra.IoC
{
    public static class NativeInjectorBootstraper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IExtractRepository, ExtractRepository>();

            services.AddScoped<IGetExtractQueryHandler, GetExtractQueryHandler>();
            services.AddDbContext<MSSQLContext>(x => x.UseSqlServer(@"Server=localhost;Database=AccountService;User Id=sa;Password=sa@12345;"));

            var mongoContext = new MongoContext
            {
                CollectionName = "Account",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "AccountDb"
            };
            var service = new EventBusService();
            service.RegisterHandle<TransferAccountCommandHandler>()
                .RegisterServices(build =>
                {
                    build.RegisterType<AccountRepository>().As<IAccountRepository>();
                    build.RegisterType<MSSQLContext>()
                        .WithParameter("options", new DbContextOptionsBuilder<MSSQLContext>()
                        .UseSqlServer(@"Server=localhost;Database=AccountService;User Id=sa;Password=sa@12345;").Options)
                        .InstancePerLifetimeScope();
                })
                .RegisterHandle<GenerateExtractCommandHandler>()
                .RegisterServices(build => {
                    build.RegisterType<ExtractRepository>().As<IExtractRepository>();
                    build.Register(c => mongoContext).As<IMongoContext>();
                })
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
            bus.Subscribe<GenerateExtractRequest, GenerateExtractCommandHandler>();

            services.AddSingleton<IMongoContext>(mongoContext);

        }
    }
}
