using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class UpdateRouteDtoValidator : AbstractValidator<UpdateRouteDto>
{
    public UpdateRouteDtoValidator()
    {
        RuleFor(x => x.RouteShortName)
            .MaximumLength(50).WithMessage("Short name must be less than 50 characters")
            .NotEmpty().WithMessage("Can't be null");
        RuleFor(x => x.RouteLongName)
            .MaximumLength(100).WithMessage("Name of city can't be superior to 100 characters")
            .NotEmpty().WithMessage("Can't be null");
        RuleFor(x => x.RouteTypeId)
            .GreaterThan(0)
            .NotEmpty().WithMessage("Must be greater than 0");
    }
}