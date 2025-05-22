namespace Travel_agency.PL.Models
{
    public record TransportRequest (
        string Type,
        string Company,
        DateTime DepartureDate,
        DateTime ArrivalDate,
        decimal Price);
}
