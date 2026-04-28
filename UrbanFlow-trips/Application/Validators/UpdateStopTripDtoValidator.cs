using FluentValidation;
using UrbanFlow_trips.Application.DTO.StopTrip;

namespace UrbanFlow_trips.Application.Validators;

public class UpdateStopTripDtoValidator : AbstractValidator<UpdateStopTripDto>
{
    public UpdateStopTripDtoValidator()
    {
        RuleFor(x => x.DepartureTime)
            .LessThan(x => x.ArrivalTime)
            .WithMessage("Departure time must be before arrival time");
    }
}