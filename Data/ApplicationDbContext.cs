using MetreOr.Models;
using Microsoft.EntityFrameworkCore;

namespace MetreOr.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}
