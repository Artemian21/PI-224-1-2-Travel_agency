namespace Travel_agency.PL.Models.Responses
{
    public record TransportResponse(
        Guid Id,
        string Type,
        string Company,
        DateTime DepartureDate,
        DateTime ArrivalDate,
        decimal Price);
}
