using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Application.Mapping;

public class TripMappingProfile : Profile
{
    public TripMappingProfile()
    {
        CreateMap<CreateTripDto, Trip>();
        CreateMap < UpdateTripServiceDto, Trip>();
    }
}