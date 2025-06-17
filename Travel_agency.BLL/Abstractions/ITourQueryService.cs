using Travel_agency.Core.BusinessModels.Tours;

namespace Travel_agency.BLL.Abstractions;

public interface ITourQueryService
{
    Task<IEnumerable<TourModel>> GetFilteredToursAsync(TourFilterModel filter);
    Task<IEnumerable<TourModel>> SearchToursAsync(string searchQuery);
}