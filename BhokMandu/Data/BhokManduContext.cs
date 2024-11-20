using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BhokMandu.Models;

namespace BhokMandu.Data
{
    public class BhokManduContext : DbContext
    {
        public BhokManduContext (DbContextOptions<BhokManduContext> options)
            : base(options)
        {
        }

        public DbSet<Food> Food { get; set; } = default!;
        public DbSet<Restaurant> Restaurant { get; set; } = default!;
        public DbSet<User> User { get; set; } = default!;
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderItem> OrderItem { get; set; }
        public DbSet<UpdateOrderStatusRequest> UpdateOrderStatusRequest { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mark UpdateOrderStatusRequest as keyless
            modelBuilder.Entity<UpdateOrderStatusRequest>().HasNoKey();
        }
    }
}