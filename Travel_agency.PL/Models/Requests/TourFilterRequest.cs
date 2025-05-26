using Travel_agency.Core.Enums;

namespace Travel_agency.PL.Models.Requests
{
    public record TourFilterRequest(
        string? Country,
        TypeTour? Type,
        DateTime? StartDateFrom,
        DateTime? StartDateTo,
        string? Name,
        string? Region,
        decimal? Price);
}
