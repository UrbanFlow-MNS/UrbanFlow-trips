using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateStopDTOValidator : AbstractValidator<CreateStopDTO>
{
    public CreateStopDTOValidator()
    {
        RuleFor(x => x.StopName)
            .NotEmpty().WithMessage("Le nom de l'arrêt ne peut pas être null")
            .MaximumLength(50).WithMessage("Nom de l'arrêt ne peut pas être supérieur à 50 caractères");
        
        RuleFor(x => x.StopLat)
            .NotNull().WithMessage("La latitude est obligatoire")
            .InclusiveBetween(-90m, 90m).WithMessage("Latitude doit être compris entre -90 et 90");

        RuleFor(x => x.StopLong)
            .NotNull().WithMessage("La longitude est obligatoire")
            .InclusiveBetween(-180m, 180m).WithMessage("Longitude doit être compris entre -180 et 180");
    }
}