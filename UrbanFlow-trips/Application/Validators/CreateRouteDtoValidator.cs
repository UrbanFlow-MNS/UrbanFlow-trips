using FluentValidation;
using UrbanFlow_trips.DTO;
using UrbanFlow_trips.Repository;

namespace UrbanFlow_trips.Application.Validators;

public class CreateRouteDtoValidator : AbstractValidator<CreateRouteDto>
{
    public CreateRouteDtoValidator(IAgencyRepository agencyRepository, IRouteTypeRepository routeTypeRepository)
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

        RuleFor(x => x.AgencyId)
            .MustAsync(async (agencyId, cancellation) =>
                !await agencyRepository.AgencyExistsAsync(agencyId, cancellation))
            .WithMessage("Agency doesn't exist");
        
        RuleFor(x => x.RouteTypeId)
            .MustAsync(async (routeTypeId, cancellation) =>
                !await routeTypeRepository.RouteTypeExistsAsync(routeTypeId, cancellation))
            .WithMessage("RouteType doesn't exist");
    }
}