using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHome.Models
{
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public double PowerUsage { get; set; }

        public string RoomId { get; set; } // Това е връзката към Room
    }
}