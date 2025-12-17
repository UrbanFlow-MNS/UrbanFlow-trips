using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Mapping;

public class AgencyMappingProfile : Profile
{
    public AgencyMappingProfile()
    {
        CreateMap<CreateAgencyDTO, Agency>();
        CreateMap<Agency, GetAgencyDTO>();
    }
}