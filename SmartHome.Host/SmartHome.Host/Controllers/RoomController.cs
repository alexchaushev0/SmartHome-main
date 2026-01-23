using SmartHome.BL.Interfaces;
using SmartHome.Models;
using SmartHome.Models.Requests;
using SmartHome.Models.Responses; 
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRooms();

            //  Mapper & Response)
            var response = rooms.Adapt<List<RoomResponse>>();

            return Ok(response);
        }

        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            if (request == null)
            {
                return BadRequest("Невалидна заявка.");
            }

            //  Mapper & Request)
            var room = request.Adapt<Room>();

            await _roomService.AddRoom(room);
            return Ok();
        }
    }
}