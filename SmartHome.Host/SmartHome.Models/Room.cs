using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHome.Models
{
    // Променяме "internal" на "public", за да се вижда от другите проекти
    public class Room
    {
        [BsonId] // Казва на MongoDB, че това е уникалният ключ
        [BsonRepresentation(BsonType.ObjectId)] // Превръща автоматично ObjectId в string
        public string Id { get; set; }

        public string Name { get; set; }

        public int Floor { get; set; }

        public int Temperature { get; set; }
    }
}