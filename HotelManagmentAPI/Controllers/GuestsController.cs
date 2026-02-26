using HotelManagmentAPI.DTOs.Guest;
using HotelManagmentAPI.Services.Guest;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HotelManagmentAPI.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]
    [ApiController]
    [Route("api/[controller]")]


    public class GuestsController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestsController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var guests = await _guestService.GetAllGuestsAsync();
            return Ok(guests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var guest = await _guestService.GetGuestByIdAsync(id);
            if (guest == null) return NotFound();
            return Ok(guest);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGuestDto dto)
        {
            var guest = await _guestService.CreateGuestAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = guest.GuestId }, guest);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateGuestDto dto)
        {
            var guest = await _guestService.UpdateGuestAsync(id, dto);
            if (guest == null) return NotFound();
            return Ok(guest);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _guestService.DeleteGuestAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}