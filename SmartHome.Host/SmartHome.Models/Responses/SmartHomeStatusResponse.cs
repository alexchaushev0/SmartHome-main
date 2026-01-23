namespace SmartHome.Models.Responses
{
    public class SmartHomeStatusResponse
    {
        public string Message { get; set; } = string.Empty;
        public DateTime CheckedAt { get; set; } = DateTime.Now;
    }
}