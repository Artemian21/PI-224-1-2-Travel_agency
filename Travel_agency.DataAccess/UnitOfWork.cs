using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Repository;

namespace Travel_agency.DataAccess;

    public class UnitOfWork : IUnitOfWork
    {
        public readonly TravelAgencyDbContext _context;
        public IHotelBookingRepository HotelBookings { get; }
        public IHotelRepository Hotels { get; }
        public IHotelRoomRepository HotelRooms { get; }
        public ITicketBookingRepository TicketBookings { get; }
        public ITourBookingRepository TourBookings { get; }
        public ITourRepository Tours { get; }
        public ITransportRepository Transports { get; }
        public IUserRepository Users { get; }

        public UnitOfWork(
            TravelAgencyDbContext context,
            IHotelBookingRepository hotelBookingRepository,
            IHotelRepository hotelRepository,
            IHotelRoomRepository hotelRoomRepository,
            ITicketBookingRepository ticketBookingRepository,
            ITourBookingRepository tourBookingRepository,
            ITourRepository tourRepository,
            ITransportRepository transportRepository,
            IUserRepository userRepository)
        {
            _context = context;

            HotelBookings = hotelBookingRepository;
            Hotels = hotelRepository;
            HotelRooms = hotelRoomRepository;
            TicketBookings = ticketBookingRepository;
            TourBookings = tourBookingRepository;
            Tours = tourRepository;
            Transports = transportRepository;
            Users = userRepository;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }