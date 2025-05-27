using AutoMapper;
using Microsoft.Extensions.Options;
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
    public class TourService : ITourService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TourService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TourDto>> GetAllToursAsync()
        {
            var tourEntities = await _unitOfWork.Tours.GetAllToursAsync();
            return _mapper.Map<IEnumerable<TourDto>>(tourEntities);
        }

        public async Task<PagedResult<TourDto>> GetPagedToursAsync(int pageNumber, int pageSize)
        {
            var totalCount = await _unitOfWork.Tours.GetTotalToursCountAsync();
            var tours = await _unitOfWork.Tours.GetToursPagedAsync(pageNumber, pageSize);
            var mappedTours = _mapper.Map<IEnumerable<TourDto>>(tours);

            return new PagedResult<TourDto>
            {
                Items = mappedTours,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };
        }

        public async Task<TourWithBookingsDto> GetTourByIdAsync(Guid tourId)
        {
            var tourEntity = await _unitOfWork.Tours.GetTourByIdAsync(tourId);
            if (tourEntity == null)
                throw new NotFoundException($"Tour with ID {tourId} not found.");

            return _mapper.Map<TourWithBookingsDto>(tourEntity);
        }

        public async Task<TourDto> AddTourAsync(TourDto tourDto)
        {
            ValidateTourDto(tourDto);

            var tourEntity = _mapper.Map<TourEntity>(tourDto);
            var addedTourEntity = await _unitOfWork.Tours.AddTourAsync(tourEntity);
            return _mapper.Map<TourDto>(addedTourEntity);
        }

        public async Task<TourDto> UpdateTourAsync(TourDto tourDto)
        {
            ValidateTourDto(tourDto);

            var tourEntity = _mapper.Map<TourEntity>(tourDto);
            var updatedTourEntity = await _unitOfWork.Tours.UpdateTourAsync(tourEntity);

            if (updatedTourEntity == null)
                throw new NotFoundException($"Tour with ID {tourDto.Id} not found.");

            return _mapper.Map<TourDto>(updatedTourEntity);
        }

        public async Task<bool> DeleteTourAsync(Guid tourId)
        {
            var tourEntity = await _unitOfWork.Tours.GetTourByIdAsync(tourId);
            if (tourEntity == null)
            {
                throw new NotFoundException($"Tour with ID {tourId} not found.");
            }

            await _unitOfWork.Tours.DeleteTourAsync(tourId);
            return true;
        }

        private void ValidateTourDto(TourDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Name))
                throw new BusinessValidationException("Tour name is required.");

            if (!Enum.IsDefined(typeof(TypeTour), dto.Type))
                throw new BusinessValidationException("Invalid tour type.");

            if (string.IsNullOrWhiteSpace(dto.Country))
                throw new BusinessValidationException("Country is required.");

            if (string.IsNullOrWhiteSpace(dto.Region))
                throw new BusinessValidationException("Region is required.");

            if (dto.StartDate.Date < DateTime.UtcNow.Date)
                throw new BusinessValidationException("Start date cannot be in the past.");

            if (dto.EndDate.Date < dto.StartDate.Date)
                throw new BusinessValidationException("End date must be after the start date.");

            if (dto.Price < 0)
                throw new BusinessValidationException("Price cannot be negative.");
        }
    }
}
