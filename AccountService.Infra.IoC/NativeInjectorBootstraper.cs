using AccountService.Domain.Repositories;
using AccountService.Infra.Data.Context;
using AccountService.Infra.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Models;
using System.Reflection;
using RabbitMQ.Abstraction;
using RabbitMQ.EventBus;
using AccountService.Domain.EventBus;
using AccountService.Infra.Data.EventBuss;
using Microsoft.Extensions.Options;
using System;
using AccountService.Domain.Handlers;
using AccountService.Domain.Interfaces.Handlers;

namespace AccountService.Infra.IoC
{
    public static class NativeInjectorBootstraper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            //services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IExtractEventBus, ExtractEventBus>();
            services.AddSingleton(new Config("localhost", 5672, "guest", "guest").Start());
            services.AddDbContext<MSSQLContext>(x => x.UseSqlServer(@"Server=localhost;Database=AccountService;User Id=sa;Password=sa@12345;"));


            services.AddTransient<ITransferAccountHandler, TransferAccountHandler>();

            services.AddSingleton<IMongoContext>(new MongoContext { 
                CollectionName = "Account",
                ConnectionString = "mongodb://localhost:27017",
                DatabaseName = "AccountDb"
            });

        }
    }
}
