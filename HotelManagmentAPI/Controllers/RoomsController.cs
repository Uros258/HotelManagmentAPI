using HotelManagmentAPI.DTOs.Room;
using HotelManagmentAPI.Services.Room;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;


namespace HotelManagmentAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]

    public class RoomsController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomsController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var rooms = await _roomService.GetAllRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("available")]
        [AllowAnonymous]

        public async Task<IActionResult> GetAvailable()
        {
            var rooms = await _roomService.GetAvailableRoomsAsync();
            return Ok(rooms);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]

        public async Task<IActionResult> GetById(int id)
        {
            var room = await _roomService.GetRoomByIdAsync(id);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateRoomDto dto)
        {
            var room = await _roomService.CreateRoomAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = room.RoomId }, room);
        }

        [HttpPatch("{id}/status")]
        [Authorize(Roles = "Admin,Receptionist,Housekeeping")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateRoomStatusDto dto)
        {
            var room = await _roomService.UpdateRoomStatusAsync(id, dto);
            if (room == null) return NotFound();
            return Ok(room);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _roomService.DeleteRoomAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}