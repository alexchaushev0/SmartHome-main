namespace SmartHome.Models.Responses
{
    public class RoomResponse
    {
        public string? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Temperature { get; set; }
    }
}