using AutoMapper;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.Core.Models.Tours;
using Travel_agency.Core.Models.Transports;
using Travel_agency.Core.Models.Users;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<HotelBookingEntity, HotelBookingDto>().ReverseMap();
        CreateMap<HotelBookingEntity, HotelBookingDetailsDto>().ReverseMap();
        CreateMap<HotelEntity, HotelDto>().ReverseMap();
        CreateMap<HotelEntity, HotelWithBookingsDto>().ReverseMap();
        CreateMap<HotelRoomEntity, HotelRoomDto>().ReverseMap();
        CreateMap<HotelRoomEntity, HotelRoomWithBookingDto>().ReverseMap();
        CreateMap<TicketBookingEntity, TicketBookingDto>().ReverseMap();
        CreateMap<TicketBookingEntity, TicketBookingDetailsDto>().ReverseMap();
        CreateMap<TourBookingEntity, TourBookingDto>().ReverseMap();
        CreateMap<TourBookingEntity, TourBookingDetailsDto>().ReverseMap();
        CreateMap<TourEntity, TourDto>().ReverseMap();
        CreateMap<TourEntity, TourWithBookingsDto>().ReverseMap();
        CreateMap<TransportEntity, TransportDto>().ReverseMap();
        CreateMap<TransportEntity, TransportWithBookingsDto>().ReverseMap();
        CreateMap<UserEntity, UserDto>().ReverseMap();
        CreateMap<UserEntity, UserWithBookingsDto>().ReverseMap();

    }
}