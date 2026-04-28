using AutoMapper;
using UrbanFlow_trips.Application.DTO.Trip;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class TripMappingProfile : Profile
{
    public TripMappingProfile()
    {
        CreateMap<CreateTripDto, Trip>();
        CreateMap < UpdateTripServiceDto, Trip>();
    }
}