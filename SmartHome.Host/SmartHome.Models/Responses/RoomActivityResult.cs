using MessagePack;

namespace SmartHome.Models.Responses
{
    [MessagePackObject]
    public class RoomActivityResult
    {
        [Key(0)]
        public Room Room { get; set; } = null!;

        [Key(1)]
        public int ActiveDevicesCount { get; set; }

        [Key(2)]
        public double EstimatedEnergyUsage { get; set; }

        [Key(3)]
        public DateTime ProcessedAt { get; set; }
    }
}