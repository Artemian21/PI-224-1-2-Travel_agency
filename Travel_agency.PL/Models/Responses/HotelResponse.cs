namespace Travel_agency.PL.Models.Responses
{
    public record HotelResponse(
        Guid Id,
        string Name,
        string Country,
        string City,
        string Address);
}
