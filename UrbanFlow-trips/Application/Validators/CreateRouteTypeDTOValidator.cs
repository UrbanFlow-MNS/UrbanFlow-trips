using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateRouteTypeDTOValidator : AbstractValidator<CreateRouteTypeDTO>
{
    public CreateRouteTypeDTOValidator()
    {
        RuleFor(x => x.RouteTypeName)
            .MaximumLength(50).WithMessage("Le nom du moyen de transport ne peut pas être supérieur à 50 caractères")
            .NotEmpty().WithMessage("Ne peut pas être vide");
        
    }
}