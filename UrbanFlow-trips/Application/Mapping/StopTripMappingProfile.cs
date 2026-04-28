using AutoMapper;
using UrbanFlow_trips.Application.DTO.StopTrip;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class StopTripMappingProfile : Profile
{
    public StopTripMappingProfile()
    {
        CreateMap<CreateStopTripDto, Stop_Trip>();
        CreateMap<UpdateStopTripDto, Stop_Trip>();
    }
}