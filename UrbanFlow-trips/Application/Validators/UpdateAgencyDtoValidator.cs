using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class UpdateAgencyDtoValidator : AbstractValidator<UpdateAgencyDto>
{

    public UpdateAgencyDtoValidator()
    {
        RuleFor(x => x.AgencyName)
            .MaximumLength(50).WithMessage("Can't be null")
            .NotEmpty().WithMessage("Name of city can't be null");

        RuleFor(x => x.TimeZone)
            .MaximumLength(14).WithMessage("Timezone can't be superior to 14 characters")
            .WithMessage("Timezone must be between 0 and 14 characters long");
    }
}