using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
//using Microsoft.EntityFrameworkCore.SqlServer;

namespace KipTrak.ContextFactory;
public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json")
        .Build();
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
        .UseSqlServer(configuration.GetConnectionString("sqlConnection"), 
        b => b.MigrationsAssembly("KipTrak"));
        return new RepositoryContext(builder.Options);
    }
}