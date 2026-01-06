using FluentValidation;
using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Application.Validators;

public class UpdateAgencyDTOValidator : AbstractValidator<UpdateTripServiceDTO>
{

    public UpdateAgencyDTOValidator()
    {
        RuleFor(x => x.serviceId);
    }
}