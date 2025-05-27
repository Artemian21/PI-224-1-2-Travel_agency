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
using Travel_agency.Core.Models.Transports;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.BLL.Services
{
    public class TicketBookingService : ITicketBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TicketBookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            this._unitOfWork = unitOfWork;
            this._mapper = mapper;
        }

        public async Task<IEnumerable<TicketBookingDto>> GetAllTicketBookingsAsync()
        {
            var ticketBookingEntities = await _unitOfWork.TicketBookings.GetAllTicketBookingsAsync();
            return _mapper.Map<IEnumerable<TicketBookingDto>>(ticketBookingEntities);
        }

        public async Task<TicketBookingDetailsDto?> GetTicketBookingByIdAsync(Guid ticketBookingId)
        {
            var ticketBookingEntity = await _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(ticketBookingId);
            if (ticketBookingEntity == null)
                throw new NotFoundException($"Ticket Booking with ID {ticketBookingId} not found.");

            return _mapper.Map<TicketBookingDetailsDto>(ticketBookingEntity);
        }

        public async Task<TicketBookingDto> AddTicketBookingAsync(TicketBookingDto ticketBookingDto)
        {
            ValidateTicketBookingDto(ticketBookingDto);

            var ticketBookingEntity = _mapper.Map<TicketBookingEntity>(ticketBookingDto);
            var addedTicketBookingEntity = await _unitOfWork.TicketBookings.AddTicketBookingAsync(ticketBookingEntity);
            return _mapper.Map<TicketBookingDto>(addedTicketBookingEntity);
        }

        public async Task<TicketBookingDto> UpdateTicketBookingAsync(TicketBookingDto ticketBookingDto)
        {
            ValidateTicketBookingDto(ticketBookingDto);

            var ticketBookingEntity = _mapper.Map<TicketBookingEntity>(ticketBookingDto);
            var updatedTicketBookingEntity = await _unitOfWork.TicketBookings.UpdateTicketBookingAsync(ticketBookingEntity);
            if (updatedTicketBookingEntity == null)
                throw new NotFoundException($"Ticket Booking with ID {ticketBookingDto.Id} not found.");


            return updatedTicketBookingEntity == null ? null : _mapper.Map<TicketBookingDto>(updatedTicketBookingEntity);
        }

        public async Task<bool> DeleteTicketBookingAsync(Guid ticketBookingId)
        {
            var ticketBookingEntity = await _unitOfWork.TicketBookings.GetTicketBookingByIdAsync(ticketBookingId);
            if (ticketBookingEntity == null)
            {
                throw new NotFoundException($"Ticket Booking with ID {ticketBookingId} not found.");
            }

            await _unitOfWork.TicketBookings.DeleteTicketBookingAsync(ticketBookingId);
            return true;
        }

        private void ValidateTicketBookingDto(TicketBookingDto dto)
        {
            if(dto == null)
                throw new ArgumentNullException(nameof(dto), "Ticket booking cannot be null.");

            if (dto.TransportId == Guid.Empty)
                throw new BusinessValidationException("TransportId is required.");

            if (dto.UserId == Guid.Empty)
                throw new BusinessValidationException("UserId is required.");

            if (!Enum.IsDefined(typeof(Status), dto.Status))
                throw new BusinessValidationException("Invalid booking status.");
        }
    }
}
