using Microsoft.EntityFrameworkCore;
using WEB_253505_AZAROV.Domain.Entities;

namespace WEB_253505_AZAROV.API.Data;

public class AppDbContext: DbContext
{
    public DbSet<Category> Categories {get; set;} = null!;
    public DbSet<Item> Items {get; set;} = null!;
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>()
            .HasOne<Category>()
            .WithMany()
            .HasForeignKey(i => i.CategoryId)
            .OnDelete(DeleteBehavior.Cascade);
        base.OnModelCreating(modelBuilder);
    }
}