using AccountService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infra.Data.Context
{
    public class MSSQLContext : DbContext
    {
        public DbSet<Account> Account { get; set; }
        public MSSQLContext(DbContextOptions<MSSQLContext> options)
           : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Account>(a =>
            {
                a.ToTable("Account");
                a.HasKey(k => k.Id);
            });
        }
    }
}
