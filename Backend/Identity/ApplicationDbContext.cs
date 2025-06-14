using Backend.Model;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Backend.Identity;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser> 
{
    public DbSet<StorageItem> StorageItems { get; set; }
    public DbSet<Tag> Tags { get; set; }
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        //builder.Entity<IdentityRole>().HasData([new IdentityRole("Admin")]);
    }
}