namespace SmartHome.Models.KafkaCache
{
    public class DatabaseCache
    {
        private readonly Dictionary<string, Room> _cache = new();
        private readonly object _lock = new();

        public void Add(Room room)
        {
            lock (_lock) _cache[room.Id] = room;
        }

        public Room? Find(string id)
        {
            lock (_lock) return _cache.TryGetValue(id, out var room) ? room : null;
        }

        public IReadOnlyCollection<Room> GetAll()
        {
            lock (_lock) return _cache.Values.ToList();
        }
    }
}