using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace EstateNexus.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<EstateNexusDbContext>
    {
        public EstateNexusDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EstateNexusDbContext>();
            optionsBuilder.UseSqlServer(DatabaseSetup.ConnectionString);

            return new EstateNexusDbContext(optionsBuilder.Options);
        }
    }
}
