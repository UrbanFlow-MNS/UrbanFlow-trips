using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateRouteDTOValidator : AbstractValidator<CreateRouteDTO>
{
    public CreateRouteDTOValidator()
    {
        RuleFor(x => x.RouteShortName)
            .MaximumLength(50).WithMessage("Nom du diminutif de la ligne doit faire moins de 50 caractères")
            .NotEmpty().WithMessage("Ne peut pas être vide");
        RuleFor(x => x.RouteLongName)
            .MaximumLength(100).WithMessage("Nom de la ligne doit faire moins de 100 caractères")
            .NotEmpty().WithMessage("Ne peut pas être vide");
        RuleFor(x => x.RouteTypeId)
            .GreaterThan(0)
            .NotEmpty().WithMessage("Doit appartenir à une ville");
    }
}