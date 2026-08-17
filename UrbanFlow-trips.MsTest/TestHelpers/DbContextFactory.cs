using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UrbanFlow_trips.Infrastructure.Database;

namespace UrbanFlow_trips.MsTest.TestHelpers;

public static class DbContextFactory
{
    public static TripsDbContext CreateInMemory()
    {
        var options = new DbContextOptionsBuilder<TripsDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;

        return new TripsDbContext(options);
    }
}
