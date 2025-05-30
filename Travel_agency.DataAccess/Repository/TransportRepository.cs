using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.DataAccess.Abstraction;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess.Repository
{
    public class TransportRepository : ITransportRepository
    {
        private readonly TravelAgencyDbContext _context;

        public TransportRepository(TravelAgencyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TransportEntity>> GetAllTransportsAsync()
        {
            return await _context.Transports.AsNoTracking().ToListAsync();
        }

        public async Task<TransportEntity> GetTransportByIdAsync(Guid transportId)
        {
            return await _context.Transports.AsNoTracking()
                                            .Include(t => t.TicketBookings)
                                            .FirstOrDefaultAsync(t => t.Id == transportId);
        }

        public async Task<TransportEntity> AddTransportAsync(TransportEntity transport)
        {
            _context.Transports.Add(transport);
            await _context.SaveChangesAsync();
            return transport;
        }

        public async Task<TransportEntity> UpdateTransportAsync(TransportEntity updatedTransport)
        {
            var existingTransport = await _context.Transports.FindAsync(updatedTransport.Id);
            if (existingTransport == null)
            {
                return null;
            }

            existingTransport.Type = updatedTransport.Type;
            existingTransport.Company = updatedTransport.Company;
            existingTransport.DepartureDate = updatedTransport.DepartureDate;
            existingTransport.ArrivalDate = updatedTransport.ArrivalDate;
            existingTransport.Price = updatedTransport.Price;

            await _context.SaveChangesAsync();

            return existingTransport;
        }

        public async Task DeleteTransportAsync(Guid transportId)
        {
            var transport = await _context.Transports.FindAsync(transportId);
            if (transport != null)
            {
                _context.Transports.Remove(transport);
                await _context.SaveChangesAsync();
            }
        }
    }
}
