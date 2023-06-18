using HandyCook.API.Models.Auth;
using Microsoft.EntityFrameworkCore;
using System.Data;
using HandyCook.API.Models;

namespace HandyCook.API
{
    public class HCContext : DbContext
    {
        public HCContext() : base()
        {
            
        }
        public HCContext(DbContextOptions options) : base(options)
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<HandyCook.API.Models.Recipe> Recipe { get; set; } = default!;
    }
}
