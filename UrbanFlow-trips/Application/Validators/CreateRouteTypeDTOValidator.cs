using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class CreateRouteTypeDTOValidator : AbstractValidator<CreateRouteTypeDTO>
{
    public CreateRouteTypeDTOValidator()
    {
        RuleFor(x => x.RouteTypeName)
            .MaximumLength(50).WithMessage("Mean fo transport must be less than 50 characters")
            .NotEmpty().WithMessage("Can't be null");
        
    }
}