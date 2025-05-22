using Travel_agency.Core.Models.Tours;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITourService
    {
        Task<TourDto> AddTourAsync(TourDto tourDto);
        Task<bool> DeleteTourAsync(Guid tourId);
        Task<IEnumerable<TourDto>> GetAllToursAsync();
        Task<PagedResult<TourDto>> GetPagedToursAsync(int pageNumber, int pageSize);
        Task<TourWithBookingsDto> GetTourByIdAsync(Guid tourId);
        Task<TourDto> UpdateTourAsync(TourDto tourDto);
    }
}