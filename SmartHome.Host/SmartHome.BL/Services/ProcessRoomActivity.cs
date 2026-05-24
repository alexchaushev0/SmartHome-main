using SmartHome.BL.Interfaces;
using SmartHome.DL.Kafka;
using SmartHome.Models.Responses;

namespace SmartHome.BL.Services
{
    internal class ProcessRoomActivity : IProcessRoomActivity
    {
        private readonly IRoomService _roomService;
        private readonly IDeviceService _deviceService;
        private readonly GenericKafkaProducer<string, RoomActivityResult> _producer;

        public ProcessRoomActivity(
            IRoomService roomService,
            IDeviceService deviceService,
            GenericKafkaProducer<string, RoomActivityResult> producer)
        {
            _roomService = roomService;
            _deviceService = deviceService;
            _producer = producer;
        }

        public async Task<RoomActivityResult> Process(string roomId)
        {
            var room = await _roomService.GetById(roomId);

            if (room == null)
                throw new ArgumentException($"Room with ID {roomId} not found.");

            var activeDevices = await _deviceService.GetActiveDevicesCount(roomId);

            // Оценка: 0.05 kWh на активно устройство
            var estimatedEnergy = activeDevices * 0.05;

            var result = new RoomActivityResult
            {
                Room = room,
                ActiveDevicesCount = activeDevices,
                EstimatedEnergyUsage = estimatedEnergy,
                ProcessedAt = DateTime.UtcNow
            };

            // Публикуваме събитието в Kafka
            await _producer.ProduceAsync(roomId, result);

            return result;
        }
    }
}