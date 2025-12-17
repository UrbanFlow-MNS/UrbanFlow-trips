using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Mapping;

public class RouteTypeMappingProfile : Profile
{
    public RouteTypeMappingProfile()
    {
        CreateMap<CreateRouteTypeDTO, RouteType>();
        CreateMap<RouteType, GetRouteTypeDTO>();
    }
}