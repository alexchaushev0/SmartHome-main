using SmartHome.BL.Interfaces;
using SmartHome.DL.Interfaces;
using SmartHome.Models;

namespace SmartHome.BL.Services
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        
        public RoomService(IRoomRepository roomRepository)
        {
            _roomRepository = roomRepository;
        }
        public async Task<Room?> GetById(string id)
        {
            
            return await _roomRepository.GetById(id);
        }
        public async Task<List<Room>> GetAllRooms()
        {
            return await _roomRepository.GetAll();
        }

        public async Task AddRoom(Room room)
        {
            await _roomRepository.Add(room);
        }
    }
}