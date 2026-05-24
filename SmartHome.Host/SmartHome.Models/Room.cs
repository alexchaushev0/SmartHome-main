using MessagePack;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SmartHome.Models
{
    [MessagePackObject]
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [Key(0)]
        public string Id { get; set; } = string.Empty;

        [Key(1)]
        public string Name { get; set; } = string.Empty;

        [Key(2)]
        public int Floor { get; set; }

        [Key(3)]
        public int Temperature { get; set; }
    }
}