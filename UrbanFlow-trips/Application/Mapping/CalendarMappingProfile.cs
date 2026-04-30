using AutoMapper;
using UrbanFlow_trips.Application.DTO.Calendar;
using UrbanFlow_trips.Domain.Entities;

namespace UrbanFlow_trips.Application.Mapping;

public class CalendarMappingProfile : Profile
{
    public CalendarMappingProfile()
    {
        CreateMap<CreateCalendarDto, Calendar>();
        CreateMap<Calendar, GetCalendarDto>();
    }
}