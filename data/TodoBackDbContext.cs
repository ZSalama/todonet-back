using Microsoft.EntityFrameworkCore;
using todo_back.Models;

namespace todo_back.Data
{
    public class TodoBackDbContext : DbContext
    {
        // Constructor called by the DI container in Program.cs
        public TodoBackDbContext(DbContextOptions<TodoBackDbContext> options)
            : base(options)
        {
        }

        // DbSet for the Weather table
        public DbSet<Weather> Weathers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration for the Weather entity
            modelBuilder.Entity<Weather>(entity =>
            {
                entity.ToTable("weather");         // Map to table name
                entity.HasKey(e => e.Id);           // Primary key

                entity.Property(e => e.Day);


                entity.Property(e => e.Temperature);

            });
        }

        // Optional: configure the connection if not using DI
        // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        // {
        //     if (!optionsBuilder.IsConfigured)
        //     {
        //         var connectionString = Environment.GetEnvironmentVariable("DefaultConnection");
        //         if (string.IsNullOrEmpty(connectionString))
        //         {
        //             throw new InvalidOperationException("Connection string 'DefaultConnection' is not set.");
        //         }
        //         optionsBuilder.UseNpgsql(connectionString);
        //     }
        // }
 
    }
}