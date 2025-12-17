using FluentValidation;
using FluentValidation.Validators;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateAgencyDTOValidator : AbstractValidator<CreateAgencyDTO>
{
    public CreateAgencyDTOValidator()
    {
        RuleFor(x => x.AgencyName)
            .MaximumLength(50)
            .NotEmpty()
            .WithMessage("Nom de la ville peut pas être nul");

        RuleFor(x => x.TimeZone)
            .MaximumLength(14)
            .WithMessage("La timezone doit être entre 1 et 14 caractères maximum");
    }
}