namespace ActiviGo.Application.Interfaces
{
    public interface IGeocodingService
    {
        Task<(double Latitude, double Longitude)?> GetCoordinatesFromAddressAsync(string address);
    }
}
