using CloudComputingProject.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CloudComputingProject.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            
        }
        public DbSet<Product> Products { get; set; }
        public DbSet<Flavor> Flavors { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<CloudComputingProject.Models.Order>? Order { get; set; }
        public DbSet<CloudComputingProject.Models.OrderDetail>? OrderDetail { get; set; }
     //   public DbSet<CloudComputingProject.Models.Product1>? Product1 { get; set; }
    }
}