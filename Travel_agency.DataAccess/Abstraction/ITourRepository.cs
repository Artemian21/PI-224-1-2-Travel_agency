using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Travel_agency.DataAccess.Entities;

namespace Travel_agency.DataAccess.Abstraction
{
    public interface ITourRepository
    {
        Task<TourEntity> AddTourAsync(TourEntity tour);
        Task DeleteTourAsync(Guid tourId);
        Task<IEnumerable<TourEntity>> GetAllToursAsync();
        Task<TourEntity> GetTourByIdAsync(Guid tourId);
        Task<TourEntity> UpdateTourAsync(TourEntity updatedTour);
    }
}
