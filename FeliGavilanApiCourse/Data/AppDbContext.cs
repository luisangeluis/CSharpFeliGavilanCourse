using FeliGavilanApiCourse.Entities;
using Microsoft.EntityFrameworkCore;

namespace FeliGavilanApiCourse.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Genre>().Property(p => p.Name).HasMaxLength(150);

        modelBuilder.Entity<Actor>().Property(p => p.Name).HasMaxLength(150);
        modelBuilder.Entity<Actor>().Property(p => p.Picture).IsUnicode(false);
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Genre> Genres { get; set; }
    
    public DbSet<Actor> Actors { get; set; }
}
