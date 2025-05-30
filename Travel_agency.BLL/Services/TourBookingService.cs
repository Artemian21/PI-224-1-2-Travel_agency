using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Enums;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Tours;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class TourBookingService : ITourBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourBookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TourBookingDto>> GetAllTourBookingsAsync()
        {
            var tourBookingEntities = await _unitOfWork.TourBookings.GetAllTourBookingsAsync();
            return _mapper.Map<IEnumerable<TourBookingDto>>(tourBookingEntities);
        }

        public async Task<TourBookingDetailsDto?> GetTourBookingByIdAsync(Guid tourBookingId)
        {
            var tourBookingEntity = await _unitOfWork.TourBookings.GetTourBookingByIdAsync(tourBookingId);
            if (tourBookingEntity == null)
                throw new NotFoundException($"Tour booking with ID {tourBookingId} not found.");


            return _mapper.Map<TourBookingDetailsDto>(tourBookingEntity);
        }

        public async Task<TourBookingDto> AddTourBookingAsync(TourBookingDto tourBookingDto)
        {
            ValidateTourBookingDto(tourBookingDto);

            var tourBookingEntity = _mapper.Map<TourBookingEntity>(tourBookingDto);
            var addedTourBookingEntity = await _unitOfWork.TourBookings.AddTourBookingAsync(tourBookingEntity);
            return _mapper.Map<TourBookingDto>(addedTourBookingEntity);
        }

        public async Task<TourBookingDto> UpdateTourBookingAsync(TourBookingDto tourBookingDto)
        {
            ValidateTourBookingDto(tourBookingDto);

            var tourBookingEntity = _mapper.Map<TourBookingEntity>(tourBookingDto);
            var updatedTourBookingEntity = await _unitOfWork.TourBookings.UpdateTourBookingAsync(tourBookingEntity);
            if (updatedTourBookingEntity == null)
                throw new NotFoundException($"Tour booking with ID {tourBookingDto.Id} not found.");

            return _mapper.Map<TourBookingDto>(updatedTourBookingEntity);
        }

        public async Task<bool> DeleteTourBookingAsync(Guid tourBookingId)
        {
            var tourBookingEntity = await _unitOfWork.TourBookings.GetTourBookingByIdAsync(tourBookingId);
            if (tourBookingEntity == null)
            {
                throw new NotFoundException($"Tour booking with ID {tourBookingId} not found.");
            }

            await _unitOfWork.TourBookings.DeleteTourBookingAsync(tourBookingId);
            return true;
        }

        private void ValidateTourBookingDto(TourBookingDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto), "Tour booking cannot be null.");

            if (dto.TourId == Guid.Empty)
                throw new BusinessValidationException("TourId is required.");

            if (dto.UserId == Guid.Empty)
                throw new BusinessValidationException("UserId is required.");

            if (!Enum.IsDefined(typeof(Status), dto.Status))
                throw new BusinessValidationException("Invalid booking status.");
        }
    }
}
