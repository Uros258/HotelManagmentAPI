using HotelManagmentAPI.DTOs.Reservation;
using HotelManagmentAPI.Services.Reservation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelManagmentAPI.Controllers
{
    [Authorize(Roles = "Admin,Receptionist")]
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationsController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reservation = await _reservationService.GetReservationByIdAsync(id);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpGet("guest/{guestId}")]
        public async Task<IActionResult> GetByGuestId(int guestId)
        {
            var reservations = await _reservationService.GetReservationsByGuestIdAsync(guestId);
            return Ok(reservations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateReservationDto dto)
        {
            var reservation = await _reservationService.CreateReservationAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = reservation.ReservationId }, reservation);
        }

        [HttpPatch("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateReservationStatusDto dto)
        {
            var reservation = await _reservationService.UpdateReservationStatusAsync(id, dto);
            if (reservation == null) return NotFound();
            return Ok(reservation);
        }

        [HttpPatch("{id}/checkin")]
        public async Task<IActionResult> CheckIn(int id)
        {
            var result = await _reservationService.CheckInAsync(id);
            if (!result) return NotFound();
            return Ok("Guest checked in successfully.");
        }

        [HttpPatch("{id}/checkout")]
        public async Task<IActionResult> CheckOut(int id)
        {
            var result = await _reservationService.CheckOutAsync(id);
            if (!result) return NotFound();
            return Ok("Guest checked out successfully.");
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var result = await _reservationService.CancelReservationAsync(id);
            if (!result) return NotFound();
            return Ok("Reservation cancelled successfully.");
        }
    }
}