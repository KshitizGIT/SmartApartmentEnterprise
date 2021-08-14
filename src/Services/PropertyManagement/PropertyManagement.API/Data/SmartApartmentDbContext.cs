using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PropertyManagement.Infrastructure.Models;

namespace PropertyManagement.API.Data
{
    public class SmartApartmentDbContext : DbContext
    {
        public DbSet<Property> Properties { get; set; }
        public DbSet<ManagementCompany> ManagementCompanies { get; set; }
        public DbSet<TaskDetails> TaskDetails { get; set; }
        public SmartApartmentDbContext(DbContextOptions options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Property>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<ManagementCompany>().Property(p => p.Id).ValueGeneratedNever();

            modelBuilder.Entity<TaskDetails>().Property(p => p.Id).ValueGeneratedNever();
            modelBuilder.Entity<TaskDetails>().Property(p => p.Status)
                .HasConversion(new EnumToStringConverter<TaskStatus>());
        }
    }
}
