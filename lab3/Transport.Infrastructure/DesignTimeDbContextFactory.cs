using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Transport.Infrastructure;

public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<TransportContext>
{
    public TransportContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TransportContext>();
        optionsBuilder.UseSqlite("Data Source=transport.db");

        return new TransportContext(optionsBuilder.Options);
    }
}