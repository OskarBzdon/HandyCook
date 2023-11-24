using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HandyCook.Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Rating> Ratings { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the RecipeId foreign key with no cascade delete
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RecipeNavigation)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.RecipeNavigationId)
                .OnDelete(DeleteBehavior.NoAction); // This is what you need to add

            // If you also have a foreign key to User, you'll need to do the same for it:
            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserNavigation)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserNavigationId)
                .OnDelete(DeleteBehavior.NoAction); // And add this too if necessary

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.UserNavigation)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserNavigationId)
                .OnDelete(DeleteBehavior.NoAction); // And add this too if necessary
        }
    }
}