using MessagePack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHome.Models
{
    [MessagePackObject]
    public class Device
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key(0)]
        public string Id { get; set; } = string.Empty;

        [Key(1)]
        public string Name { get; set; } = string.Empty;

        [Key(2)]
        public string Type { get; set; } = string.Empty;

        [Key(3)]
        public double PowerUsage { get; set; }

        [Key(4)]
        public string RoomId { get; set; } = string.Empty;
    }
}