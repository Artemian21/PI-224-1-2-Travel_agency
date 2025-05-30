using AutoMapper;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.Core.Models.Tours;
using Travel_agency.Core.Models.Transports;
using Travel_agency.Core.Models.Users;
using Travel_agency.PL.Models.Requests;
using Travel_agency.PL.Models.Responses;

namespace Travel_agency.PL
{
    public class PLMappingProfile : Profile
    {
        public PLMappingProfile()
        {
            CreateMap<UserRequest, UserDto>();
            CreateMap<UserDto, UserResponse>();

            CreateMap<RegisterUserRequest,  RegisterUserDto>();

            CreateMap<TransportRequest, TransportDto>();
            CreateMap<TransportDto, TransportResponse>();

            CreateMap<TourRequest, TourDto>();
            CreateMap<TourDto, TourResponse>();
            CreateMap<TourFilterRequest , TourFilterDto>();

            CreateMap<TourBookingRequest, TourBookingDto>();
            CreateMap<TourBookingDto, TourBookingResponse>();
            CreateMap<TourBookingDetailsDto, TourBookingDetailsResponse>();

            CreateMap<TicketBookingRequest, TicketBookingDto>();
            CreateMap<TicketBookingDto, TicketBookingResponse>();

            CreateMap<HotelRoomRequest, HotelRoomDto>();
            CreateMap<HotelRoomDto, HotelRoomResponse>();

            CreateMap<HotelRequest, HotelDto>();
            CreateMap<HotelDto, HotelResponse>();

            CreateMap<HotelBookingRequest, HotelBookingDto>();
            CreateMap<HotelBookingDto, HotelBookingResponse>();
            CreateMap<HotelBookingDetailsDto, HotelBookingDetailsResponse>();

        }
    }
}
