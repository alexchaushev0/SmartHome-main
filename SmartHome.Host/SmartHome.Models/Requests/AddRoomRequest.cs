namespace SmartHome.Models.Requests
{
    public class AddRoomRequest
    {
        public string Name { get; set; } = string.Empty;
        public double Temperature { get; set; }
    }
}