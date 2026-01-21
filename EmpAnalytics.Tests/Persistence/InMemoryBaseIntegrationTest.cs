using EmpAnalytics.Persistence;
using Microsoft.EntityFrameworkCore;

public abstract class InMemoryBaseIntegrationTest
{
    protected static ApplicationDbContext CreateContext(string dbName)
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        return new ApplicationDbContext(options);
    }
}