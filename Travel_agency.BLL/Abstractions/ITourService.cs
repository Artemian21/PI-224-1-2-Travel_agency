using Travel_agency.Core.BusinessModels.Tours;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITourService
    {
        Task<TourModel> AddTourAsync(TourModel tourModel);
        Task<bool> DeleteTourAsync(Guid tourId);
        Task<IEnumerable<TourModel>> GetAllToursAsync();
        Task<PagedResult<TourModel>> GetPagedToursAsync(int pageNumber, int pageSize);
        Task<TourWithBookingsModel> GetTourByIdAsync(Guid tourId);
        Task<TourModel> UpdateTourAsync(TourModel tourModel);
    }
}