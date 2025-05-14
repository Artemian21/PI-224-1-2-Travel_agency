using AutoMapper;
using Travel_agency.Core.Models;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess;

public class MappingProfile: Profile
{
    public MappingProfile()
    {
        CreateMap<HotelBookingEntity, HotelBookingDto>().ReverseMap(); 
        CreateMap<HotelEntity, HotelDto>().ReverseMap(); 
        CreateMap<HotelRoomEntity, HotelRoomDto>().ReverseMap();
        CreateMap<TicketBookingEntity, TicketBookingDto>().ReverseMap(); 
        CreateMap<TourBookingEntity, TourBookingDto>().ReverseMap(); 
        CreateMap<TourEntity, TourDto>().ReverseMap(); 
        CreateMap<TransportEntity, TransportDto>().ReverseMap(); 
        CreateMap<UserEntity, UserDto>().ReverseMap(); 
        
    }
}