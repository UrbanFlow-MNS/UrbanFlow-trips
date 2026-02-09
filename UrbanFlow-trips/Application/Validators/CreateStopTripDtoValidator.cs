using FluentValidation;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Application.Validators;

public class CreateStopTripDtoValidator : AbstractValidator<CreateStopTripDto>
{
    public CreateStopTripDtoValidator(IStopRepository stopRepository)
    {
        RuleFor(x => x.DepartureTime)
            .LessThan(x => x.ArrivalTime)
            .WithMessage("Departure time must be before arrival time");
        
    }
}