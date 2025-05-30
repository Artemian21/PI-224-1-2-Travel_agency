using Travel_agency.Core.Models.Tours;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITourBookingService
    {
        Task<TourBookingDto> AddTourBookingAsync(TourBookingDto tourBookingDto);
        Task<bool> DeleteTourBookingAsync(Guid tourBookingId);
        Task<IEnumerable<TourBookingDto>> GetAllTourBookingsAsync();
        Task<TourBookingDetailsDto?> GetTourBookingByIdAsync(Guid tourBookingId);
        Task<TourBookingDto?> UpdateTourBookingAsync(TourBookingDto tourBookingDto);
    }
}