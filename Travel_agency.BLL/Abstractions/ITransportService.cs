using Travel_agency.Core.BusinessModels.Transports;

namespace Travel_agency.BLL.Abstractions
{
    public interface ITransportService
    {
        Task<TransportModel> AddTransportAsync(TransportModel transportModel);
        Task<bool> DeleteTransportAsync(Guid transportId);
        Task<IEnumerable<TransportModel>> GetAllTransportsAsync();
        Task<TransportWithBookingsModel> GetTransportByIdAsync(Guid transportId);
        Task<TransportModel> UpdateTransportAsync(TransportModel transportModel);
    }
}