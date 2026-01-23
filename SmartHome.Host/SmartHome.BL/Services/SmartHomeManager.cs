using SmartHome.BL.Interfaces;

namespace SmartHome.BL.Services
{
    public class SmartHomeManager : ISmartHomeManager
    {
        private readonly IRoomService _roomService;
        private readonly IDeviceService _deviceService;


        public SmartHomeManager(IRoomService roomService, IDeviceService deviceService)
        {
            _roomService = roomService;
            _deviceService = deviceService;
        }

        public async Task<string> GetRoomStatus(string roomId)
        {
            var room = await _roomService.GetById(roomId);

            if (room == null)
            {
                throw new KeyNotFoundException($"Room with ID {roomId} was not found.");
            }

            var devices = await _deviceService.GetActiveDevicesCount(roomId);
            return $"Стая: {room.Name} има {devices} активни устройства.";
        }
    }
}