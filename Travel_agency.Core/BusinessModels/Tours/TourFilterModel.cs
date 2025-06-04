using Travel_agency.Core.Enums;

namespace Travel_agency.Core.BusinessModels.Tours;

public class TourFilterModel
{
    public string? Country { get; set; }
    public TypeTour? Type { get; set; }
    public DateTime? StartDateFrom { get; set; }
    public DateTime? StartDateTo { get; set; }
    public string? Name { get; set; }
    public string? Region { get; set; }
    public decimal? Price { get; set; }
}

