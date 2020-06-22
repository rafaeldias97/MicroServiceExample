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

namespace AccountService.Infra.IoC
{
    public static class NativeInjectorBootstraper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IExtractEventBus, ExtractEventBus>();
            services.AddSingleton(new Config("localhost", 5672, "mqadmin", "Admin123XX_").Start());
            services.AddDbContext<MSSQLContext>(x => x.UseSqlServer(@"Server=localhost;Database=dbmodel;User Id=sa;Password=sa@12345;"));

        }
    }
}
