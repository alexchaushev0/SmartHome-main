namespace SmartHome.BL.Interfaces
{
    public interface IDeviceService
    {
        Task<int> GetActiveDevicesCount(string roomId);
    }
}