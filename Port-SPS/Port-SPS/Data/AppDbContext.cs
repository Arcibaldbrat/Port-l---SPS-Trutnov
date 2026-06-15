using Microsoft.EntityFrameworkCore;
using Port_SPS.Models;

namespace Port_SPS.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<AppUser> Users => Set<AppUser>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasIndex(user => user.Username).IsUnique();
            entity.HasIndex(user => user.Email).IsUnique();
            entity.Property(user => user.Role).HasMaxLength(32);
            entity.Property(user => user.Username).HasMaxLength(80);
            entity.Property(user => user.Email).HasMaxLength(160);
        });
    }
}
