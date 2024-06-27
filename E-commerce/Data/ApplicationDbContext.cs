using System;
using E_Commerce.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce.data
{
	public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Slider> Sliders { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed superAdmin user
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "superAdmin",
                Password = "123456", 
                Email = "superadmin@gmail.com",
                FirstName = "Super",
                LastName = "Admin",
                Role = "superadmin"
            });

            modelBuilder.Entity<Order>()
                .HasMany(o => o.OrderItems)
                .WithOne(oi => oi.Order)
                .HasForeignKey(oi => oi.OrderId);

            modelBuilder.Entity<OrderItem>()
                .HasOne(oi => oi.Product)
                .WithMany()
                .HasForeignKey(oi => oi.ProductId);

            modelBuilder.Entity<Slider>()
               .HasOne(s => s.Product)
               .WithMany()
               .HasForeignKey(s => s.ProductId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApplicationDbContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApplicationDbContext>>()))
            {
                // Look for any users.
                if (context.Users.Any())
                {
                    return;   // DB has been seeded
                }

                context.Users.AddRange(
                    new User
                    {
                        Username = "superAdmin",
                        Password = "123456",
                        Email = "superadmin@gmail.com",
                        FirstName = "Super",
                        LastName = "Admin",
                        Role = "superadmin"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}

