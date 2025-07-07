using FeliGavilanApiCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeliGavilanApiCourse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Genre> Genres { get; set; }
}
