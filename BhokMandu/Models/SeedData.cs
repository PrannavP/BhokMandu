using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BhokMandu.Data;
using System;
using System.Linq;

namespace BhokMandu.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            Console.WriteLine("Seeding....... LOOLLLL");
            using (var context = new BhokManduContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BhokManduContext>>()))
            {
                // Check if restaurants already exist
                if (context.User.Any())
                {
                    Console.WriteLine("Database already seeded.");
                    return;
                }

                // Seed restaurants
                Console.WriteLine("Seeding database...");
                context.User.AddRange(
                    new User
                    {
                        FullName = "Admin Test",
                        Email = "admin@admin.com",
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin"),
                        Role = "Admin"
                    }
                );
                context.SaveChanges();
                Console.WriteLine("Seeding completed.");
            }
        }
    }
}