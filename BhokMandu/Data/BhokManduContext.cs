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
    }
}