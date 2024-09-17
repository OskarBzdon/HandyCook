using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HandyCook.Application.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Step> Steps { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.RecipeNavigation)
                .WithMany(b => b.Ratings)
                .HasForeignKey(r => r.RecipeNavigationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Rating>()
                .HasOne(r => r.UserNavigation)
                .WithMany(u => u.Ratings)
                .HasForeignKey(r => r.UserNavigationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Recipe>()
                .HasOne(r => r.UserNavigation)
                .WithMany(u => u.Recipes)
                .HasForeignKey(r => r.UserNavigationId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Step>()
                .HasOne(s => s.RecipeNavigation)
                .WithMany(r => r.Steps)
                .HasForeignKey(s => s.RecipeNavigationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.RecipeNavigation)
                .WithMany(r => r.Ingredients)
                .HasForeignKey(i => i.RecipeNavigationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ingredient>()
                .HasOne(i => i.StepNavigation)
                .WithMany(s => s.Ingredients)
                .HasForeignKey(i => i.StepNavigationId)
                .IsRequired(false)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}