using Fishward.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Fishward.Data;

public class AppDbContext : DbContext
{
    public AppDbContext() { }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options) { }
    
    
    public virtual DbSet<Player> Players { get; set; }
    public virtual DbSet<PlayerResources> PlayerResources { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(e => e.DiscordID);
            entity.ToTable("players");
            
            entity.HasOne(d => d.PlayerResource).WithOne(p => p.Player)
                .HasForeignKey<Player>(d => d.DiscordID)
                .OnDelete(DeleteBehavior.Restrict);
        });
        
        modelBuilder.Entity<PlayerResources>(entity =>
        {
            entity.HasKey(e => e.DiscordID);
            entity.ToTable("player_resources");
            
            entity.HasOne(d => d.Player).WithOne(p => p.PlayerResource)
                .HasForeignKey<PlayerResources>(d => d.DiscordID)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
    
}
