using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateRouteDTOValidator : AbstractValidator<CreateRouteDTO>
{
    public CreateRouteDTOValidator()
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
        
        /* Exemple pour vérifier en bdd si ça existe déjà ou pas
        RuleFor(x => x.Email)
            .MustAsync(async (email, cancellation) =>
                !await userRepo.EmailExists(email))
        */
    }
}