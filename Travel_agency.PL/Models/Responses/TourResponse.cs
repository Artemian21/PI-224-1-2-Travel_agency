using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Responses
{
    public record TourResponse(
        Guid Id,
        string Name,
        TypeTour Type,
        string Country,
        string Region,
        DateTime StartDate,
        DateTime EndDate,
        decimal Price,
        string? ImageUrl);
}
