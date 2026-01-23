using SmartHome.BL.Interfaces;
using SmartHome.Models.Responses; // Добавено
using Microsoft.AspNetCore.Mvc;

namespace SmartHome.Host.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SmartHomeController : ControllerBase
    {
        private readonly ISmartHomeManager _smartHomeManager;

        public SmartHomeController(ISmartHomeManager smartHomeManager)
        {
            _smartHomeManager = smartHomeManager;
        }

        [HttpGet("GetStatus/{id}")]
        public async Task<IActionResult> GetStatus(string id)
        {
            var statusMessage = await _smartHomeManager.GetRoomStatus(id);

            
            var response = new SmartHomeStatusResponse
            {
                Message = statusMessage
            };

            return Ok(response);
        }
    }
}