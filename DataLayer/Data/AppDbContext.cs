using DataLayer.Configurations;
using DataLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Data
{
    public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
    {
        public DbSet<Payment> Payments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfiguration(new PaymentConfigurations());
        }
    }
}
