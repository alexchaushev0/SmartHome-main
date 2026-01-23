using SmartHome.BL.Interfaces;

namespace SmartHome.BL.Services
{
    public class DeviceService : IDeviceService
    {
        public async Task<int> GetActiveDevicesCount(string roomId)
        {
            // Връщаме примерно число, за да имитираме логика
            return await Task.FromResult(new Random().Next(1, 10));
        }
    }
}