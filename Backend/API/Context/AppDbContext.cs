using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Context;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Staff> Staff { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Staff>()
            .HasIndex(s => s.StaffId)
            .IsUnique();
    }
}