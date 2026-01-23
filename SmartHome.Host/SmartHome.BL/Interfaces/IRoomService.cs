using SmartHome.Models;

namespace SmartHome.BL.Interfaces
{
    public interface IRoomService
    {
        Task<List<Room>> GetAllRooms();

        Task AddRoom(Room room);

        Task<Room?> GetById(string id);
    }
}