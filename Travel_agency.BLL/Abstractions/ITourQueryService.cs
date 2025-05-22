using Travel_agency.Core.Models.Tours;

namespace Travel_agency.BLL.Abstractions;

public interface ITourQueryService
{
    Task<IEnumerable<TourDto>> GetFilteredToursAsync(TourFilterDto filter);
    Task<IEnumerable<TourDto>> SearchToursAsync(string searchQuery);
}