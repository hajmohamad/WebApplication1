using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Data;

public class ApplicationDbContext : IdentityDbContext<AppUser>
{


    public DbSet<User> Users { get; set; }
    public DbSet<Admin> Admins { get; set; }
  public ApplicationDbContext(DbContextOptions dbContextOptions)
        : base(dbContextOptions)
    {
      
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder){
        base.OnModelCreating(modelBuilder);

        List<IdentityRole> roles = new List<IdentityRole>();
        roles.Add(new IdentityRole("Admin"));
        roles.Add(new IdentityRole("User"));
        modelBuilder.Entity<IdentityRole>().HasData(roles);
    }




}