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

        // DbSet for the Todo table
        public DbSet<Todo> Todos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Fluent API configuration for the Todo entity
            modelBuilder.Entity<Todo>(entity =>
            {
                entity.ToTable("todos");         // Map to table name
                entity.HasKey(e => e.Id);           // Primary key

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(100); // Title is required and has a max length

                entity.Property(e => e.Category)
                    .IsRequired()
                    .HasMaxLength(50); // Category is required and has a max length

                entity.Property(e => e.Description)
                    .HasMaxLength(500); // Description is optional with a max length

                entity.Property(e => e.DueDate);
                    
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