using FeliGavilanApiCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeliGavilanApiCourse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genre>().Property(p=>p.Name).HasMaxLength(150);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Genre> Genres { get; set; }
}
