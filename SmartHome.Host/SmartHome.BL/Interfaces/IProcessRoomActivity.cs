using SmartHome.Models.Responses;

namespace SmartHome.BL.Interfaces
{
    public interface IProcessRoomActivity
    {
        Task<RoomActivityResult> Process(string roomId);
    }
}