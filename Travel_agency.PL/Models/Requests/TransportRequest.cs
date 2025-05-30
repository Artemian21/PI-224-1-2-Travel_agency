namespace Travel_agency.PL.Models.Requests
{
    public record TransportRequest(
        string Type,
        string Company,
        DateTime DepartureDate,
        DateTime ArrivalDate,
        decimal Price);
}
