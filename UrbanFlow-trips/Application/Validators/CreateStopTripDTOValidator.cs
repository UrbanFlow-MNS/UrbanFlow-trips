using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateStopTripDTOValidator : AbstractValidator<CreateStopTripDTO>
{
    public CreateStopTripDTOValidator()
    {
        RuleFor(x => x.DepartureTime)
            .LessThan(x => x.ArrivalTime)
            .WithMessage("Departure time must be before arrival time");
    }
}