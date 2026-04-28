using UrbanFlow_trips.Application.DTO.CompleteRoute;
using UrbanFlow_trips.Application.DTO.Route;

namespace UrbanFlow_trips.Domain.Interfaces;

public interface IGetAdjustedRoutesUseCase
{
    Task<List<GetCompleteRouteDto>> ExecuteAsync(RouteFilterDto filter);
}