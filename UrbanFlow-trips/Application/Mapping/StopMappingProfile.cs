using AutoMapper;
using UrbanFlow_trips.Application.DTO.Stop;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class StopMappingProfile : Profile
{
    public StopMappingProfile()
    {
        CreateMap<CreateStopDto, Stop_Times>();
        CreateMap<Stop_Times, GetStopDto>();
        CreateMap<UpdateStopDto, Stop_Times>();
    }
}