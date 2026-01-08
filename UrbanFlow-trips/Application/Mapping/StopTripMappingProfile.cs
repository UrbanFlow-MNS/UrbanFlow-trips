using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Application.Mapping;

public class StopTripMappingProfile : Profile
{
    public StopTripMappingProfile()
    {
        CreateMap<CreateStopTripDto, Stop_Trip>();
        CreateMap<UpdateStopTripDto, Stop_Trip>();
    }
}