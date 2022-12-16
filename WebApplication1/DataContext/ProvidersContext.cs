using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.DataContext
{
    public class ProvidersContext : DbContext
    {
        public DbSet<Provider> Provider { get; set; } = null!;
        public ProvidersContext(DbContextOptions<ProvidersContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    
    }
}
