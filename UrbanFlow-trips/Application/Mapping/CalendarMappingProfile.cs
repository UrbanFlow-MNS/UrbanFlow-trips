using AutoMapper;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Models;

namespace UrbanFlow_trips.Application.Mapping;

public class CalendarMappingProfile : Profile
{
    public CalendarMappingProfile()
    {
        CreateMap<CreateCalendarDTO, Calendar>();
        CreateMap<Calendar, GetCalendarDTO>();
    }
}