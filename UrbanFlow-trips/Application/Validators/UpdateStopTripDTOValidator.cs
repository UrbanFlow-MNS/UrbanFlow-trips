using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class UpdateStopTripDTOValidator : AbstractValidator<UpdateStopTripDTO>
{
    public UpdateStopTripDTOValidator()
    {
        RuleFor(x => x.DepartureTime)
            .LessThan(x => x.ArrivalTime)
            .WithMessage("Departure time must be before arrival time");
    }
}