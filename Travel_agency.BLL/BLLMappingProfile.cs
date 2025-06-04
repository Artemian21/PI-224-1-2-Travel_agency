using AutoMapper;
using Travel_agency.Core.BusinessModels.Hotels;
using Travel_agency.Core.BusinessModels.Tours;
using Travel_agency.Core.BusinessModels.Transports;
using Travel_agency.Core.BusinessModels.Users;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL;

public class BLLMappingProfile : Profile
{
    public BLLMappingProfile()
    {
        CreateMap<HotelBookingEntity, HotelBookingModel>().ReverseMap();
        CreateMap<HotelBookingEntity, HotelBookingDetailsModel>().ReverseMap();
        CreateMap<HotelEntity, HotelModel>().ReverseMap();
        CreateMap<HotelEntity, HotelWithBookingsModel>().ReverseMap();
        CreateMap<HotelRoomEntity, HotelRoomModel>().ReverseMap();
        CreateMap<HotelRoomEntity, HotelRoomWithBookingModel>().ReverseMap();
        CreateMap<TicketBookingEntity, TicketBookingModel>().ReverseMap();
        CreateMap<TicketBookingEntity, TicketBookingDetailsModel>().ReverseMap();
        CreateMap<TourBookingEntity, TourBookingModel>().ReverseMap();
        CreateMap<TourBookingEntity, TourBookingDetailsModel>().ReverseMap();
        CreateMap<TourEntity, TourModel>().ReverseMap();
        CreateMap<TourEntity, TourWithBookingsModel>().ReverseMap();
        CreateMap<TransportEntity, TransportModel>().ReverseMap();
        CreateMap<TransportEntity, TransportWithBookingsModel>().ReverseMap();
        CreateMap<UserEntity, UserModel>().ReverseMap();
        CreateMap<UserEntity, UserWithBookingsModel>().ReverseMap();
    }
}