using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

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