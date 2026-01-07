using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Application.Mapping;

public class RouteMappingProfile : Profile
{ 
    public RouteMappingProfile()
    {
        CreateMap<CreateRouteDTO, Routes>();
        CreateMap<Routes, GetRouteDTO>();
        CreateMap<UpdateRouteDTO, Routes>();
    }
}