using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions;

public interface ITourQueryService
{
    Task<IEnumerable<TourDto>> GetFilteredToursAsync(TourFilterDto filter);
}