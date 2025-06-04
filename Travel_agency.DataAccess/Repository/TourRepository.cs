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
    public class TourRepository : ITourRepository
    {
        private readonly TravelAgencyDbContext _context;

        public TourRepository(TravelAgencyDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourEntity>> GetAllToursAsync()
        {
            return await _context.Tours.AsNoTracking().ToListAsync();
        }

        public async Task<IEnumerable<TourEntity>> GetToursPagedAsync(int pageNumber, int pageSize)
        {
            return await _context.Tours
                .AsNoTracking()
                .OrderBy(t => t.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalToursCountAsync()
        {
            return await _context.Tours.CountAsync();
        }

        public async Task<TourEntity> GetTourByIdAsync(Guid tourId)
        {
            return await _context.Tours.AsNoTracking()
                                       .Include(t => t.TourBookings)
                                       .FirstOrDefaultAsync(t => t.Id == tourId);
        }

        public async Task<TourEntity> AddTourAsync(TourEntity tour)
        {
            _context.Tours.Add(tour);
            return tour;
        }

        public async Task<TourEntity> UpdateTourAsync(TourEntity updatedTour)
        {
            var existingTour = await _context.Tours.FindAsync(updatedTour.Id);
            if (existingTour == null)
            {
                return null;
            }

            existingTour.Name = updatedTour.Name;
            existingTour.StartDate = updatedTour.StartDate;
            existingTour.EndDate = updatedTour.EndDate;
            existingTour.Type = updatedTour.Type;
            existingTour.Country = updatedTour.Country;
            existingTour.Region = updatedTour.Region;
            existingTour.ImageUrl = updatedTour.ImageUrl;
            existingTour.Price = updatedTour.Price;

            return existingTour;
        }

        public async Task DeleteTourAsync(Guid tourId)
        {
            var tour = await _context.Tours.FindAsync(tourId);
            if (tour != null)
            {
                _context.Tours.Remove(tour);
            }
        }
    }
}
