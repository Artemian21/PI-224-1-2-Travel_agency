using AutoMapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Exceptions;
using Travel_agency.Core.Models.Transports;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class TransportService : ITransportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TransportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TransportDto>> GetAllTransportsAsync()
        {
            var transportEntities = await _unitOfWork.Transports.GetAllTransportsAsync();
            return _mapper.Map<IEnumerable<TransportDto>>(transportEntities);
        }

        public async Task<TransportWithBookingsDto> GetTransportByIdAsync(Guid transportId)
        {
            var transportEntity = await _unitOfWork.Transports.GetTransportByIdAsync(transportId);
            if (transportEntity == null)
                throw new NotFoundException($"Transport with ID {transportId} not found.");

            return _mapper.Map<TransportWithBookingsDto>(transportEntity);
        }

        public async Task<TransportDto> AddTransportAsync(TransportDto transportDto)
        {
            ValidateTransportDto(transportDto);

            var transportEntity = _mapper.Map<TransportEntity>(transportDto);
            var addedTransportEntity = await _unitOfWork.Transports.AddTransportAsync(transportEntity);
            return _mapper.Map<TransportDto>(addedTransportEntity);
        }

        public async Task<TransportDto> UpdateTransportAsync(TransportDto transportDto)
        {
            ValidateTransportDto(transportDto);

            var transportEntity = _mapper.Map<TransportEntity>(transportDto);
            var updatedTransportEntity = await _unitOfWork.Transports.UpdateTransportAsync(transportEntity);

            if (updatedTransportEntity == null)
                throw new NotFoundException($"Transport with ID {transportDto.Id} not found.");

            return _mapper.Map<TransportDto>(updatedTransportEntity);
        }

        public async Task<bool> DeleteTransportAsync(Guid transportId)
        {
            var transportEntity = await _unitOfWork.Transports.GetTransportByIdAsync(transportId);
            if (transportEntity == null)
            {
                throw new NotFoundException($"Transport with ID {transportId} not found.");
            }

            await _unitOfWork.Transports.DeleteTransportAsync(transportId);
            return true;
        }

        private void ValidateTransportDto(TransportDto dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (dto.Price < 0)
                throw new BusinessValidationException("Price cannot be negative.");

            if (string.IsNullOrWhiteSpace(dto.Company))
                throw new BusinessValidationException("Company name is required.");

            if (string.IsNullOrWhiteSpace(dto.Type))
                throw new BusinessValidationException("Transport type is required.");

            if (dto.DepartureDate < DateTime.UtcNow)
                throw new BusinessValidationException("Departure date cannot be in the past.");

            if (dto.ArrivalDate < dto.DepartureDate)
                throw new BusinessValidationException("Arrival date must be after the departure date.");
        }
    }
}
