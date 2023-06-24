using BookShoppingCart.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace BookShoppingCart.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<Genre> Genre { get; set; }
        public DbSet<Book> Book { get; set; }
        public DbSet<ShoppingCart> ShoppingCart { get; set; }
        public DbSet<CartDetail> CartDetail { get; set; }
        
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<OrderStatus>().HasData
                (
                new OrderStatus
                {
                    Id = 7,
                    StatusId = 7,
                    StatusName = "Lost"
                },
                new OrderStatus
                {
                    Id = 1,
                    StatusId = 1,
                    StatusName = "Pending"
                },
                new OrderStatus
                {
                    Id = 2,
                    StatusId=2,
                    StatusName="Shipped"
                },
                new OrderStatus
                {
                    Id = 3,
                    StatusId=3,
                    StatusName= "Delivered"
                },
                new OrderStatus
                {
                    Id = 4,
                    StatusId = 4,
                    StatusName = "Cancelled"
                },
                new OrderStatus
                {
                    Id = 5,
                    StatusId = 5,
                    StatusName = "Returned"
                },
                new OrderStatus
                {
                    Id = 6,
                    StatusId = 6,
                    StatusName = "Refund"
                }
                

                );
            base.OnModelCreating(builder);
        }


    }
}