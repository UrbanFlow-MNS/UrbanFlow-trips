using FluentValidation;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Application.Validators;

public class CreateTripDtoValidator : AbstractValidator<CreateTripDto>
{
    public CreateTripDtoValidator(IRoutesRepository routesRepository, ICalendarRepository calendarRepository)
    {
        RuleFor(x => x.RouteId)
            .MustAsync(async (routeId, cancellation) =>
                !await routesRepository.RouteExistsAsync(routeId, cancellation))
            .WithMessage("Route doesn't exist");
        
        RuleFor(x => x.ServiceId)
            .MustAsync(async (serviceId, cancellation) =>
                !await calendarRepository.CalendarExistsAsync(serviceId, cancellation))
            .WithMessage("Service doesn't exist");
        
    }
}