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
    public class HotelBookingService : IHotelBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelBookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<HotelBookingDto>> GetAllHotelBookingsAsync()
        {
            var hotelBookingsEntities = await _unitOfWork.HotelBookings.GetAllHotelBookingsAsync();
            return _mapper.Map<IEnumerable<HotelBookingDto>>(hotelBookingsEntities);
        }

        public async Task<HotelBookingDto> GetHotelBookingByIdAsync(Guid hotelBookingId)
        {
            var hotelBookingEntity = await _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(hotelBookingId);
            if (hotelBookingEntity == null)
            {
                throw new NotFoundException($"Hotel booking with ID {hotelBookingId} not found.");
            }

            return _mapper.Map<HotelBookingDto>(hotelBookingEntity);
        }

        public async Task<HotelBookingDto> AddHotelBookingAsync(HotelBookingDto hotelBookingDto)
        {
            ValidateHotelBookingDto(hotelBookingDto);

            var hotelBookingEntity = _mapper.Map<HotelBookingEntity>(hotelBookingDto);
            var addedHotelBookingEntity = await _unitOfWork.HotelBookings.AddHotelBookingAsync(hotelBookingEntity);
            return _mapper.Map<HotelBookingDto>(addedHotelBookingEntity);
        }

        public async Task<HotelBookingDto> UpdateHotelBookingAsync(HotelBookingDto hotelBookingDto)
        {
            ValidateHotelBookingDto(hotelBookingDto);

            var hotelBookingEntity = _mapper.Map<HotelBookingEntity>(hotelBookingDto);
            var updatedHotelBookingEntity = await _unitOfWork.HotelBookings.UpdateHotelBookingAsync(hotelBookingEntity);
            if (updatedHotelBookingEntity == null)
            {
                throw new NotFoundException($"Hotel booking with ID {hotelBookingDto.Id} not found.");
            }

            return _mapper.Map<HotelBookingDto>(updatedHotelBookingEntity);
        }

        public async Task<bool> DeleteHotelBookingAsync(Guid hotelBookingId)
        {
            var hotelBookingEntity = await _unitOfWork.HotelBookings.GetHotelBookingByIdAsync(hotelBookingId);
            if (hotelBookingEntity == null)
            {
                throw new NotFoundException($"Hotel booking with ID {hotelBookingId} not found.");
            }

            await _unitOfWork.HotelBookings.DeleteHotelBookingAsync(hotelBookingId);
            return true;
        }

        private void ValidateHotelBookingDto(HotelBookingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Booking object cannot be null.");

            if (dto.HotelRoomId == Guid.Empty)
                throw new BusinessValidationException("Hotel room ID is required.");

            if (dto.UserId == Guid.Empty)
                throw new BusinessValidationException("User ID is required.");

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
                throw new BusinessValidationException("Start date cannot be in the past.");

            if (dto.EndDate.Date <= dto.StartDate.Date)
                throw new BusinessValidationException("End date must be after start date.");

            if (dto.NumberOfGuests <= 0)
                throw new BusinessValidationException("Number of guests must be greater than 0.");

            if (!Enum.IsDefined(typeof(Status), dto.Status))
                throw new BusinessValidationException("Invalid booking status.");
        }
    }
}
