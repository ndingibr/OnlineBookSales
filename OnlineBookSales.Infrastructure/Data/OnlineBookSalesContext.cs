using Microsoft.EntityFrameworkCore;
using OnlineBookSales.Core.Entities;
using OnlineBookSales.Core.Entities.Map;


namespace OnlineBookSales.Infrastructure
{
    public class OnlineBookSalesContext : DbContext
    {
        public OnlineBookSalesContext(DbContextOptions<OnlineBookSalesContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            new UsersMap(modelBuilder.Entity<Users>());
            new BooksMap(modelBuilder.Entity<Books>());
            new SubscriptionsMap(modelBuilder.Entity<Subscriptions>());
            ;
        }
    }
}
