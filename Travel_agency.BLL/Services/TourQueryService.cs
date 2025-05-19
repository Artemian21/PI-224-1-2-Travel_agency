using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Travel_agency.BLL.Abstractions;
using Travel_agency.Core.Models;
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
        var query = _unitOfWork.Tours.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.SearchQuery))
        {
            var search = filter.SearchQuery.Trim().ToLower();

            query = query.Where(t =>
                EF.Functions.Like(t.Name.ToLower(), $"%{search}%") || 
            EF.Functions.Like(t.Type.ToString().ToLower(), $"%{search}%") || 
            EF.Functions.Like(t.Country.ToLower(), $"%{search}%") || 
            EF.Functions.Like(t.Region.ToLower(), $"%{search}%") ||
            EF.Functions.Like(t.StartDate.ToString("yyyy-MM-dd"), $"%{search}%")); // по бажанню
        }

        if (!string.IsNullOrWhiteSpace(filter.Country))
        {
            var country = filter.Country.Trim().ToLower();
            query = query.Where(t => t.Country.ToLower() == country);
        }

        if (filter.Type.HasValue)
        {
            query = query.Where(t => t.Type == filter.Type.Value);
        }

        if (!string.IsNullOrWhiteSpace(filter.Region))
        {
            var region = filter.Region.Trim().ToLower();
            query = query.Where(t => t.Region.ToLower() == region);
        }

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim().ToLower();
            query = query.Where(t => t.Name.ToLower().Contains(name));
        }

        if (filter.StartDateFrom.HasValue)
        {
            query = query.Where(t => t.StartDate >= filter.StartDateFrom.Value);
        }

        if (filter.StartDateTo.HasValue)
        {
            query = query.Where(t => t.StartDate <= filter.StartDateTo.Value);
        }

        if (filter.Price.HasValue)
        {
            query = query.Where(t => t.Price <= filter.Price.Value);
        }

        return await query
            .ProjectTo<TourDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }
}