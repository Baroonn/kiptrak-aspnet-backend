using Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class RepositoryContext: IdentityDbContext<AppUser>
{
    public RepositoryContext(DbContextOptions options)
        :base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<AppUser>()
        .HasOne(u => u.Profile)
        .WithOne(p => p.User)
        .HasForeignKey<UserProfile>(b => b.UserId);
    }

    public DbSet<Assignment>? Assignments { get; set; }
    public DbSet<UserProfile>? Profiles { get; set; }
}