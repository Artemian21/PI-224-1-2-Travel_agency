using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Hotels;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class HotelService : IHotelService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<HotelDto>> GetAllHotelsAsync()
        {
            var hotelEntities = await _unitOfWork.Hotels.GetAllHotelsAsync();
            return _mapper.Map<IEnumerable<HotelDto>>(hotelEntities);
        }

        public async Task<HotelWithBookingsDto?> GetHotelByIdAsync(Guid hotelId)
        {
            var hotelEntity = await _unitOfWork.Hotels.GetHotelByIdAsync(hotelId);
            if (hotelEntity == null)
                throw new NotFoundException($"Hotel with ID {hotelId} not found.");

            return _mapper.Map<HotelWithBookingsDto>(hotelEntity);
        }

        public async Task<HotelDto> AddHotelAsync(HotelDto hotelDto)
        {
            ValidateHotelDto(hotelDto);

            var hotelEntity = _mapper.Map<HotelEntity>(hotelDto);
            var addedHotelEntity = await _unitOfWork.Hotels.AddHotelAsync(hotelEntity);
            return _mapper.Map<HotelDto>(addedHotelEntity);
        }

        public async Task<HotelDto> UpdateHotelAsync(HotelDto hotelDto)
        {
            ValidateHotelDto(hotelDto);

            var hotelEntity = _mapper.Map<HotelEntity>(hotelDto);
            var updatedHotelEntity = await _unitOfWork.Hotels.UpdateHotelAsync(hotelEntity);
            if (updatedHotelEntity == null)
                throw new NotFoundException($"Hotel with ID {hotelDto.Id} not found.");

            return _mapper.Map<HotelDto>(updatedHotelEntity);
        }

        public async Task<bool> DeleteHotelAsync(Guid hotelId)
        {
            var hotelEntity = await _unitOfWork.Hotels.GetHotelByIdAsync(hotelId);
            if (hotelEntity == null)
            {
                throw new NotFoundException($"Hotel with ID {hotelId} not found.");
            }

            await _unitOfWork.Hotels.DeleteHotelAsync(hotelId);
            return true;
        }

        private void ValidateHotelDto(HotelDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Hotel object cannot be null.");

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessValidationException("Hotel name is required.");

            if (string.IsNullOrWhiteSpace(dto.Country))
                throw new BusinessValidationException("Country is required.");

            if (string.IsNullOrWhiteSpace(dto.City))
                throw new BusinessValidationException("City is required.");

            if (string.IsNullOrWhiteSpace(dto.Address))
                throw new BusinessValidationException("Address is required.");
        }
    }
}
