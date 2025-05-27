using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models.Tours;
using Travel_agency.DataAccess.Abstraction;

namespace Travel_agency.BLL.Services;

public class TourQueryService : ITourQueryService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public TourQueryService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TourDto>> GetFilteredToursAsync(TourFilterDto filter)
    {
        var tours = await _unitOfWork.Tours.GetAllToursAsync();

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            var country = filter.Country.Trim().ToLower();
            tours = tours.Where(t => t.Country?.ToLower() == country).ToList();
        }

        if (filter.Type.HasValue)
        {
            tours = tours.Where(t => t.Type == filter.Type.Value).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Region))
        {
            var region = filter.Region.Trim().ToLower();
            tours = tours.Where(t => t.Region?.ToLower() == region).ToList();
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim().ToLower();
            tours = tours.Where(t => t.Name?.ToLower().Contains(name) == true).ToList();
        }

        if (filter.StartDateFrom.HasValue)
        {
            tours = tours.Where(t => t.StartDate >= filter.StartDateFrom.Value).ToList();
        }

        if (filter.StartDateTo.HasValue)
        {
            tours = tours.Where(t => t.StartDate <= filter.StartDateTo.Value).ToList();
        }

        if (filter.Price.HasValue)
        {
            tours = tours.Where(t => t.Price <= filter.Price.Value).ToList();
        }

        return _mapper.Map<IEnumerable<TourDto>>(tours);
    }

    public async Task<IEnumerable<TourDto>> SearchToursAsync(string searchQuery)
    {
        var tours = await _unitOfWork.Tours.GetAllToursAsync();

        if (!string.IsNullOrWhiteSpace(searchQuery))
        {
            var search = searchQuery.Trim().ToLower();

            tours = tours.Where(t =>
                (t.Name?.ToLower().Contains(search) ?? false) ||
                (t.Type.ToString().ToLower().Contains(search)) ||
                (t.Country?.ToLower().Contains(search) ?? false) ||
                (t.Region?.ToLower().Contains(search) ?? false)
            ).ToList();

            if (DateTime.TryParse(search, out var parsedDate))
            {
                tours = tours.Where(t => t.StartDate.Date == parsedDate.Date).ToList();
            }
        }

        return _mapper.Map<IEnumerable<TourDto>>(tours);
    }
}