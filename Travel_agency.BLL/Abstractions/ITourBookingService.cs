using Travel_agency.Core.BusinessModels.Tours;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITourBookingService
    {
        Task<TourBookingModel> AddTourBookingAsync(TourBookingModel tourBookingModel);
        Task<bool> DeleteTourBookingAsync(Guid tourBookingId);
        Task<IEnumerable<TourBookingModel>> GetAllTourBookingsAsync();
        Task<TourBookingDetailsModel?> GetTourBookingByIdAsync(Guid tourBookingId);
        Task<TourBookingModel?> UpdateTourBookingAsync(TourBookingModel tourBookingModel);
    }
}