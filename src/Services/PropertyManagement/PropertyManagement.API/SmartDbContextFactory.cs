using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using PropertyManagement.API.Data;
using System;

namespace PropertyManagement.API
{
    public class SmartDbContextFactory : IDesignTimeDbContextFactory<SmartApartmentDbContext>
    {
        public SmartApartmentDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SmartApartmentDbContext>();
            optionsBuilder.UseSqlite("Data Source=SmartApartmentDb.db");
            return new SmartApartmentDbContext(optionsBuilder.Options);
        }
    }
}
