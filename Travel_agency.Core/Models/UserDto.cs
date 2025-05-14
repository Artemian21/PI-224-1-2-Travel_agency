using Travel_agency.Core.Enums;

namespace Travel_agency.Core.Models;

public class UserDto
{
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public UserRole Role { get; set; } = UserRole.Registered;

        public ICollection<HotelBookingDto> HotelBookings { get; set; } = new List<HotelBookingDto>();
        public ICollection<TicketBookingDto> TicketBookings { get; set; } = new List<TicketBookingDto>();
        public ICollection<TourBookingDto> TourBookings { get; set; } = new List<TourBookingDto>();
}