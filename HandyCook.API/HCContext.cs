using HandyCook.API.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HandyCook.API.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace HandyCook.API
{
    public class HCContext : IdentityDbContext<User>
    {
        public HCContext() : base()
        {
            
        }
        public HCContext(DbContextOptions options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<HandyCook.API.Models.Recipe> Recipe { get; set; } = default!;
    }
}
