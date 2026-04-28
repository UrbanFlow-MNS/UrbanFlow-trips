using UrbanFlow_trips.DTO;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface IGetAdjustedRoutesUseCase
{
    Task<List<GetCompleteRouteDto>> ExecuteAsync(RouteFilterDto filter);
}