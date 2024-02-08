using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class DataContext : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
    IdentityRoleClaim<int>, IdentityUserToken<int>>
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<FriendRequest> Requests { get; set; }
    public DbSet<Message> Messages { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<Connection> Connections { get; set; }
    public DbSet<Photo> Photos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        modelBuilder.Entity<AppRole>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.Role)
            .HasForeignKey(ur => ur.RoleId)
            .IsRequired();

        modelBuilder.Entity<AppUser>()
            .HasMany(ur => ur.UserRoles)
            .WithOne(u => u.User)
            .HasForeignKey(ur => ur.UserId)
            .IsRequired();

        modelBuilder.Entity<FriendRequest>()
            .HasKey(k => new { k.SourceUserId, k.TargetUserId });

        modelBuilder.Entity<FriendRequest>()
            .HasOne(s => s.SourceUser)
            .WithMany(r => r.RequestedUsers)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<FriendRequest>()
            .HasOne(t => t.TargetUser)
            .WithMany(r => r.RequestedByUsers)
            .HasForeignKey(t => t.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(u => u.Recipient)
            .WithMany(m => m.MessageReceived)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(u => u.Sender)
            .WithMany(m => m.MessageSent)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Photo>()
            .HasQueryFilter(u => u.IsApproved);

    }
}
