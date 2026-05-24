using SmartHome.DL.Interfaces;
using SmartHome.DL.Kafka;
using SmartHome.Models;

namespace SmartHome.DL.DbCacheReader
{
    public class DbCacheReaderService : IDbCacheReaderService
    {
        private readonly IRoomRepository _roomRepo;
        private readonly GenericKafkaProducer<string, Room> _producer;

        public DbCacheReaderService(
            IRoomRepository roomRepo,
            GenericKafkaProducer<string, Room> producer)
        {
            _roomRepo = roomRepo;
            _producer = producer;
        }

        public async Task ReadAndPublishAsync(CancellationToken cancellationToken)
        {
            var rooms = await _roomRepo.GetAll();
            foreach (var room in rooms)
                await _producer.ProduceAsync(room.Id, room);
        }
    }
}