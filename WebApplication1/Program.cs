using Microsoft.EntityFrameworkCore;
using WebApplication1.DataContext;

var builder = WebApplication.CreateBuilder(args);

string connection = "Server=DESKTOP-GF8REUK\\SQLEXPRESS;Database=Cafe;Trusted_Connection=true;Encrypt=False";

builder.Services.AddDbContext<DishesContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<ProvidersContext>(options => options.UseSqlServer(connection));
builder.Services.AddDbContext<SalesContext>(options => options.UseSqlServer(connection));

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.MapControllerRoute(name: "default", pattern: "{controller=Dishes}/{action=Index}/{id?}");

app.Run();
