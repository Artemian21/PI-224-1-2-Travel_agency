using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class HotelRoomService : IHotelRoomService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelRoomService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<HotelRoomDto>> GetAllHotelRoomsAsync()
        {
            var hotelRoomEntities = await _unitOfWork.HotelRooms.GetAllHotelRoomsAsync();
            return _mapper.Map<IEnumerable<HotelRoomDto>>(hotelRoomEntities);
        }

        public async Task<HotelRoomDto?> GetHotelRoomByIdAsync(Guid hotelRoomId)
        {
            var hotelRoomEntity = await _unitOfWork.HotelRooms.GetHotelRoomByIdAsync(hotelRoomId);
            if (hotelRoomEntity == null)
            {
                throw new NotFoundException($"Hotel room with ID {hotelRoomId} not found.");
            }

            return _mapper.Map<HotelRoomDto>(hotelRoomEntity);
        }

        public async Task<HotelRoomDto> AddHotelRoomAsync(HotelRoomDto hotelRoomDto)
        {
            ValidateHotelRoomDto(hotelRoomDto);

            var hotelRoomEntity = _mapper.Map<HotelRoomEntity>(hotelRoomDto);
            var addedHotelRoomEntity = await _unitOfWork.HotelRooms.AddHotelRoomAsync(hotelRoomEntity);
            return _mapper.Map<HotelRoomDto>(addedHotelRoomEntity);
        }

        public async Task<HotelRoomDto> UpdateHotelRoomAsync(HotelRoomDto hotelRoomDto)
        {
            ValidateHotelRoomDto(hotelRoomDto);

            var hotelRoomEntity = _mapper.Map<HotelRoomEntity>(hotelRoomDto);
            var updatedHotelRoomEntity = await _unitOfWork.HotelRooms.UpdateHotelRoomAsync(hotelRoomEntity);
            if (updatedHotelRoomEntity == null)
            {
                throw new NotFoundException($"Hotel room with ID {hotelRoomDto.Id} not found.");
            }

            return _mapper.Map<HotelRoomDto>(updatedHotelRoomEntity);
        }

        public async Task<bool> DeleteHotelRoomAsync(Guid hotelRoomId)
        {
            var hotelRoomEntity = await _unitOfWork.HotelRooms.GetHotelRoomByIdAsync(hotelRoomId);
            if (hotelRoomEntity == null)
            {
                throw new NotFoundException($"Hotel room with ID {hotelRoomId} not found.");
            }

            await _unitOfWork.HotelRooms.DeleteHotelRoomAsync(hotelRoomId);
            return true;
        }

        private void ValidateHotelRoomDto(HotelRoomDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Hotel room cannot be null.");

            if (!Enum.IsDefined(typeof(RoomType), dto.RoomType))
                throw new BusinessValidationException("Invalid room type.");

            if (dto.Capacity <= 0)
                throw new BusinessValidationException("Room capacity must be greater than zero.");

            if (dto.PricePerNight < 0)
                throw new BusinessValidationException("Price per night cannot be negative.");

            if (dto.HotelId == Guid.Empty)
                throw new BusinessValidationException("Hotel ID is required.");
        }
    }
}
