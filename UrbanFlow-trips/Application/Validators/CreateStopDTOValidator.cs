using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateStopDTOValidator : AbstractValidator<CreateStopDTO>
{
    public CreateStopDTOValidator()
    {
        RuleFor(x => x.StopName)
            .NotEmpty().WithMessage("Stop name can't be null")
            .MaximumLength(50).WithMessage("Stop name must be less than 50 characters");
        
        RuleFor(x => x.StopLat)
            .NotNull().WithMessage("Latitude is mandatory")
            .InclusiveBetween(-90m, 90m).WithMessage("Latitude must be between -90 and 90");

        RuleFor(x => x.StopLong)
            .NotNull().WithMessage("Longitude is mandatory")
            .InclusiveBetween(-180m, 180m).WithMessage("Longitude must be between -180 and 180");
    }
}