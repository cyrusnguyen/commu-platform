using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
    public DbSet<FriendRequest> Requests { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

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
    }
}
