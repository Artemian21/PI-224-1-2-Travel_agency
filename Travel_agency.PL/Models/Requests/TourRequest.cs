using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record TourRequest(
        string Name,
        TypeTour Type,
        string Country,
        string Region,
        DateTime StartDate,
        DateTime EndDate,
        decimal Price,
        string? ImageUrl);
}
