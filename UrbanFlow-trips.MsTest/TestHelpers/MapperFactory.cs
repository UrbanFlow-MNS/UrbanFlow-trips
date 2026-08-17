using AutoMapper;
using Microsoft.Extensions.Logging.Abstractions;
using UrbanFlow_trips.Application.Mapping;

namespace UrbanFlow_trips.MsTest.TestHelpers;

public static class MapperFactory
{
    public static IMapper Create()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<CalendarMappingProfile>();
            cfg.AddProfile<IncidentMappingProfile>();
            cfg.AddProfile<RouteMappingProfile>();
            cfg.AddProfile<StopMappingProfile>();
            cfg.AddProfile<StopTripMappingProfile>();
            cfg.AddProfile<TripMappingProfile>();
        }, NullLoggerFactory.Instance);

        return config.CreateMapper();
    }
}
