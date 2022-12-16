using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DataContext
{
    public class SalesContext : DbContext
    {
        public DbSet<Sales> Sales { get; set; } = null!;
        public SalesContext(DbContextOptions<SalesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    
    }
}
