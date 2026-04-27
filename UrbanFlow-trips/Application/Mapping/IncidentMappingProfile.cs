using AutoMapper;
using UrbanFlow_trips.DTO.Incident;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Application.Mapping;

public class IncidentMappingProfile : Profile
{
    public IncidentMappingProfile()
    {
        CreateMap<CreateIncidentDto, Incident>();
    }
}