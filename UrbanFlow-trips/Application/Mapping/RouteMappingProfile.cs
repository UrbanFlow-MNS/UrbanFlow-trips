using AutoMapper;
using UrbanFlow_trips.Application.DTO.Route;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class RouteMappingProfile : Profile
{ 
    public RouteMappingProfile()
    {
        CreateMap<CreateRouteDto, Routes>();
        CreateMap<Routes, GetRouteDto>();
        CreateMap<UpdateRouteDto, Routes>();
    }
}