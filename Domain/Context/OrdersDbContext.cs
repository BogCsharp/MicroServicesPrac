using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Domain.Context
{
    public sealed class OrdersDbContext : DbContext
    {
        public OrdersDbContext(DbContextOptions<OrdersDbContext> options) : base(options)
        {
            if (Database.GetPendingMigrations().Any())
            {
                Database.Migrate();
            }
        }

        public DbSet<CustomerEntity> Customers { get; set; } = null!;
        public DbSet<CartEntity> Carts { get; set; } = null!;
        public DbSet<CartItemEntity> CartsItems { get; set; } = null!;
        public DbSet<OrderEntity> Orders { get; set; } = null!;
    }


}
