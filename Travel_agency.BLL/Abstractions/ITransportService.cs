using Travel_agency.Core.Models;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITransportService
    {
        Task<TransportDto> AddTransportAsync(TransportDto transportDto);
        Task<bool> DeleteTransportAsync(Guid transportId);
        Task<IEnumerable<TransportDto>> GetAllTransportsAsync();
        Task<TransportDto> GetTransportByIdAsync(Guid transportId);
        Task<TransportDto> UpdateTransportAsync(TransportDto transportDto);
    }
}