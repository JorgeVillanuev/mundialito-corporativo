using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Mundialito.Infrastructure.Persistencia;

public class MundialitoDbContextFactory : IDesignTimeDbContextFactory<MundialitoDbContext>
{
    public MundialitoDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<MundialitoDbContext>();

        optionsBuilder.UseSqlServer("Server=localhost,1433;Database=Mundialito;User Id=sa;Password=Mundialito2026!;TrustServerCertificate=True");

        return new MundialitoDbContext(optionsBuilder.Options);
    }
}
