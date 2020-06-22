using AccountService.Domain.Repositories;
using AccountService.Infra.Data.Context;
using AccountService.Infra.Data.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace AccountService.Infra.IoC
{
    public static class NativeInjectorBootstraper
    {
        public static void RegisterServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddDbContext<MSSQLContext>(x => x.UseSqlServer(@"Server=localhost;Database=dbmodel;User Id=sa;Password=sa@12345;"));

        }
    }
}
