using Microsoft.EntityFrameworkCore;
using ProductCategoryApp.Models;

namespace ProductCategoryApp.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
}