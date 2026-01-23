using SmartHome.Models;

namespace SmartHome.DL.Interfaces
{
    public interface IRoomRepository
    {
        // Взима всички стаи
        Task<List<Room>> GetAll();

        // Добавя нова стая
        Task Add(Room room);

        // Взима конкретна стая по ID (Връща null, ако не я намери)
        Task<Room?> GetById(string id);

        // Изтрива стая по ID
        Task Delete(string id);
    }
}