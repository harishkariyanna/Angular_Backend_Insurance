using Microsoft.EntityFrameworkCore;
using insu.Models;

namespace insu.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Policy> Policies { get; set; }
    public DbSet<Claim> Claims { get; set; }
    public DbSet<Upload> Uploads { get; set; }
    public DbSet<PolicyApplication> PolicyApplications { get; set; }
    public DbSet<Review> Reviews { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>()
            .HasOne(c => c.PolicyApplication)
            .WithMany()
            .HasForeignKey(c => c.PolicyApplicationId)
            .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Claim>()
            .HasOne(c => c.User)
            .WithMany()
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PolicyApplication>()
            .HasOne(pa => pa.User)
            .WithMany()
            .HasForeignKey(pa => pa.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<PolicyApplication>()
            .HasOne(pa => pa.AssignedAgent)
            .WithMany()
            .HasForeignKey(pa => pa.AgentId)
            .OnDelete(DeleteBehavior.NoAction);

    }
}