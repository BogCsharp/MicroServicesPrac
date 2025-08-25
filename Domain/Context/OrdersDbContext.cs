using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IdentityRole = Domain.Entities.IdentityRole;

namespace Domain.Context
{
    public sealed class OrdersDbContext : IdentityDbContext<UserEntity,IdentityRole,long>
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
        public DbSet<MerchantEntity> Merchants { get; set; } = null!;
    }


}
