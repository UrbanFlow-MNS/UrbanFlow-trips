using AutoMapper;
using UrbanFlow_trips.Application.DTO.Incident;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class IncidentMappingProfile : Profile
{
    public IncidentMappingProfile()
    {
        CreateMap<CreateIncidentDto, Incident>();
    }
}