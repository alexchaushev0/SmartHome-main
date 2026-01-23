using SmartHome.DL.Interfaces;
using SmartHome.Models;
using SmartHome.Models.Configurations;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace SmartHome.DL.Repositories
{
    public class RoomMongoRepository : IRoomRepository
    {
        private readonly IMongoCollection<Room> _rooms;

        public RoomMongoRepository(IOptionsMonitor<MongoDbConfiguration> settings)
        {
            var client = new MongoClient(settings.CurrentValue.ConnectionString);
            var database = client.GetDatabase(settings.CurrentValue.DatabaseName);
            _rooms = database.GetCollection<Room>("Rooms");
        }

        public async Task<List<Room>> GetAll() =>
            await _rooms.Find(r => true).ToListAsync();

        public async Task Add(Room room) =>
            await _rooms.InsertOneAsync(room);

        // Добавихме ?, за да може да връща null, ако стаята не съществува
        public async Task<Room?> GetById(string id) =>
            await _rooms.Find(r => r.Id == id).FirstOrDefaultAsync();

        public async Task Delete(string id) =>
            await _rooms.DeleteOneAsync(r => r.Id == id);
    }
}