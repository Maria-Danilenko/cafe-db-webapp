using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace WebApplication1.DataContext
{
    public class DishesContext : DbContext
    {
        public DbSet<Dish> Dish { get; set; } = null!;
        public DishesContext(DbContextOptions<DishesContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}