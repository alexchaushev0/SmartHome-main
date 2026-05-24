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
        private readonly IProcessRoomActivity _processRoomActivity;

        public RoomController(
            IRoomService roomService,
            IProcessRoomActivity processRoomActivity)
        {
            _roomService = roomService;
            _processRoomActivity = processRoomActivity;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRooms();
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

            var room = request.Adapt<Room>();
            await _roomService.AddRoom(room);
            return Ok();
        }

        [HttpPost("ProcessActivity")]
        public async Task<IActionResult> ProcessActivity([FromQuery] string roomId)
        {
            if (string.IsNullOrWhiteSpace(roomId))
                return BadRequest("Room ID must be provided.");

            try
            {
                var result = await _processRoomActivity.Process(roomId);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}